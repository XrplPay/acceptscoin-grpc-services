using System;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.Services.PosServer.Domain.Interfaces;
using AcceptsCoin.Services.PosServer.Domain.Models;
using AcceptsCoin.Services.PosServer.Protos;
using Grpc.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace AcceptsCoin.Services.PosServer.Services
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class StoreGrpcService : StoreAppService.StoreAppServiceBase
    {
        private readonly ILogger<StoreGrpcService> _logger;
        private IStoreRepository _storeRepository;
        public StoreGrpcService(ILogger<StoreGrpcService> logger, IStoreRepository storeRepository)
        {
            _logger = logger;
            _storeRepository = storeRepository;
        }

        private Guid getUserId(ServerCallContext context)
        {
            return Guid.Parse(context.GetHttpContext().User.Identity.Name);
        }
        private string getPartnetId(ServerCallContext context)
        {
            return "bff3b2dd-e89d-46fc-a868-aab93a3efbbe";
        }
        public override async Task<StoreListGm> GetAll(StoreQueryFilter request, ServerCallContext context)
        {
            StoreListGm response = new StoreListGm();

            IQueryable<Store> query = _storeRepository.GetQuery();


            response.CurrentPage = request.PageId;
            response.ItemCount = await _storeRepository.GetCount(query);
            response.PageCount = (response.ItemCount / request.PageSize) + 1;


            var stores = from store in await _storeRepository.GetAll(query, request.PageId, request.PageSize)
                         select new StoreGm()
                         {
                             Id = store.StoreId.ToString(),
                             DefaultCurrency = store.DefaultCurrency,
                             Email = store.Email,
                             Name = store.Name,
                             RefundDay = store.RefundDay,
                             Threshold = store.Threshold,
                             WebSite = store.WebSite,
                         };
            response.Items.AddRange(stores.ToArray());
            return await Task.FromResult(response);
        }


        public override async Task<StoreGm> GetById(StoreIdFilter request, ServerCallContext context)
        {
            var store = await _storeRepository.Find(request.StoreId);
            var searchedStore = new StoreGm()
            {
                Id = store.StoreId.ToString(),
                WebSite = store.WebSite,
                Threshold = store.Threshold,
                RefundDay = store.RefundDay,
                Name = store.Name,
                Email = store.Email,
                DefaultCurrency = store.DefaultCurrency,
            };
            return await Task.FromResult(searchedStore);
        }

        public override async Task<StoreGm> Post(CreateStoreGm request, ServerCallContext context)
        {
            var storeAdded = new Store()
            {
                StoreId = Guid.NewGuid(),
                DefaultCurrency = request.DefaultCurrency,
                Email = request.Email,
                Name = request.Name,
                RefundDay = request.RefundDay,
                Threshold = request.Threshold,
                WebSite = request.WebSite,
                CreatedById = getUserId(context),
                CreatedDate = DateTime.Now,
                Published = false,
                Deleted = false,
            };

            var store = await _storeRepository.Add(storeAdded);

            var response = new StoreGm()
            {
                Id = store.StoreId.ToString(),
                WebSite = store.WebSite,
                Threshold = store.Threshold,
                RefundDay = store.RefundDay,
                Name = store.Name,
                Email = store.Email,
                DefaultCurrency = store.DefaultCurrency,
            };
            return await Task.FromResult(response);
        }

        public override async Task<WoocommerceStoreGm> WoocommercePost(WoocommerceCreateStoreGm request, ServerCallContext context)
        {

            var userId = Guid.Parse(request.UserId);

            var webSiteUrl = request.WebSite.Replace("https://", "").Replace("http://", "");

            Store store = await _storeRepository.FindByWebSiteUrl(webSiteUrl);
            if (store == null)
            {

                var storeAdded = new Store()
                {
                    StoreId = Guid.NewGuid(),
                    DefaultCurrency = request.DefaultCurrency,
                    Email = request.Email,
                    Name = request.Name,
                    RefundDay = 30,
                    Threshold = 0,
                    WebSite = request.WebSite,
                    CreatedById = userId,
                    CreatedDate = DateTime.Now,
                    Published = false,
                    Deleted = false,
                };

                store = await _storeRepository.Add(storeAdded);

                var response = new WoocommerceStoreGm()
                {
                    Email = store.Email,
                };
                return await Task.FromResult(response);
            }
            else
            {
                //Send Email to Email Address

                store.Published = false;
                store.UpdatedDate = DateTime.Now;
                store.UpdatedById = userId;

                store = await _storeRepository.Update(store);

                var response = new WoocommerceStoreGm()
                {
                    Email = store.Email,
                };
                return await Task.FromResult(response);
            }
        }


        public override async Task<StoreGm> Put(UpdateStoreGm request,
           ServerCallContext context)
        {
            Store store = await _storeRepository.Find(request.Id);
            if (store == null)
            {
                return await Task.FromResult<StoreGm>(null);
            }


            store.Name = request.Name;
            store.RefundDay = request.RefundDay;
            store.Threshold = request.Threshold;
            store.UpdatedById = getUserId(context);
            store.UpdatedDate = DateTime.Now;

            store = await _storeRepository.Update(store);

            return await Task.FromResult<StoreGm>(new StoreGm()
            {
                Id = store.StoreId.ToString(),
                WebSite = store.WebSite,
                Threshold = store.Threshold,
                RefundDay = store.RefundDay,
                Name = store.Name,
                Email = store.Email,
                DefaultCurrency = store.DefaultCurrency,
            });
        }


        public override async Task<EmptyStore> UpdatePublishStatus(StoreIdFilter request,
           ServerCallContext context)
        {
            Store store = await _storeRepository.Find(request.StoreId);
            if (store == null)
            {
                return await Task.FromResult<EmptyStore>(null);
            }

            store.Published = !store.Published;
            store.UpdatedById = getUserId(context);
            store.UpdatedDate = DateTime.Now;

            await _storeRepository.Update(store);
            return await Task.FromResult<EmptyStore>(new EmptyStore());
        }



        public override async Task<EmptyStore> Delete(StoreIdFilter request, ServerCallContext context)
        {
            Store store = await _storeRepository.Find(request.StoreId);
            if (store == null)
            {
                return await Task.FromResult<EmptyStore>(null);
            }

            await _storeRepository.Delete(store);
            return await Task.FromResult<EmptyStore>(new EmptyStore());
        }

        public override async Task<EmptyStore> SoftDelete(StoreIdFilter request, ServerCallContext context)
        {
            Store store = await _storeRepository.Find(request.StoreId);

            if (store == null)
            {
                return await Task.FromResult<EmptyStore>(null);
            }

            store.Deleted = true;
            store.UpdatedById = getUserId(context);
            store.UpdatedDate = DateTime.Now;

            await _storeRepository.Update(store);
            return await Task.FromResult<EmptyStore>(new EmptyStore());
        }
        public override async Task<EmptyStore> SoftDeleteCollection(StoreDeleteCollectionGm request, ServerCallContext context)
        {

            foreach (var item in request.Items)
            {
                Store store = await _storeRepository.Find(item.StoreId);

                if (store == null)
                {
                    return await Task.FromResult<EmptyStore>(null);
                }

                store.Deleted = true;
                store.UpdatedById = getUserId(context);
                store.UpdatedDate = DateTime.Now;

                await _storeRepository.Update(store);
            }

            return await Task.FromResult<EmptyStore>(new EmptyStore());
        }
    }
}
