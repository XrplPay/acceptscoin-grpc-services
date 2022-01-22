using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.Services.DirectoryServer.Domain.Interfaces;
using AcceptsCoin.Services.DirectoryServer.Domain.Models;
using AcceptsCoin.Services.DirectoryServer.Protos;
using Grpc.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Google.Protobuf.WellKnownTypes;
using Empty = Google.Protobuf.WellKnownTypes.Empty;

namespace AcceptsCoin.Services.DirectoryServer
{

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BusinessGalleryGrpcService : BusinessGalleryAppService.BusinessGalleryAppServiceBase
    {
        private readonly ILogger<BusinessGalleryGrpcService> _logger;
        private IBusinessGalleryRepository _businessGalleryRepository;
        private ICategoryRepository  _categoryRepository;
        public BusinessGalleryGrpcService(ILogger<BusinessGalleryGrpcService> logger, IBusinessGalleryRepository businessGalleryRepository, ICategoryRepository categoryRepository)
        {
            _logger = logger;
            _businessGalleryRepository = businessGalleryRepository;
            _categoryRepository = categoryRepository;
        }

        private Guid getUserId(ServerCallContext context)
        {
            return Guid.Parse(context.GetHttpContext().User.Identity.Name);
        }
        private string getPartnetId(ServerCallContext context)
        {
            return "bff3b2dd-e89d-46fc-a868-aab93a3efbbe";
        }

        public override async Task<BusinessGalleryListGm> GetAll(BusinessGalleryQueryFilter request, ServerCallContext context)
        {
            BusinessGalleryListGm response = new BusinessGalleryListGm();

            IQueryable<BusinessGallery> query = _businessGalleryRepository.GetQuery();


            response.CurrentPage = request.PageId;
            response.ItemCount = await _businessGalleryRepository.GetCount(query);
            response.PageCount = (response.ItemCount / request.PageSize) + 1;



            var BusinessGalleryes = from BusinessGallery in await _businessGalleryRepository.GetAll(query, request.PageId, request.PageSize)
                                    select new BusinessGalleryGm()
                                    {
                                        Id = BusinessGallery.BusinessGalleryId.ToString(),
                                        Extension = BusinessGallery.Extension,
                                        Name = BusinessGallery.Name,

                                    };

            response.Items.AddRange(BusinessGalleryes.ToArray());
            return await Task.FromResult(response);

        }

        [AllowAnonymous]
        public override async Task<BusinessGalleryListGm> GetByBusinessId(BusinessGalleryBusinessIdFilter request, ServerCallContext context)
        {
            BusinessGalleryListGm response = new BusinessGalleryListGm();

            IQueryable<BusinessGallery> query = _businessGalleryRepository.GetQuery();
            query = query.Where(x => x.BusinessId == Guid.Parse(request.BusienssId));

            response.CurrentPage = request.PageId;
            response.ItemCount = await _businessGalleryRepository.GetCount(query);
            response.PageCount = (response.ItemCount / request.PageSize) + 1;



            var BusinessGalleryes = from BusinessGallery in await _businessGalleryRepository.GetAll(query, request.PageId, request.PageSize)
                                    select new BusinessGalleryGm()
                                    {
                                        Id = BusinessGallery.BusinessGalleryId.ToString(),
                                        Extension = BusinessGallery.Extension,
                                        Name = BusinessGallery.Name,
                                        Url = BusinessGallery.Name + BusinessGallery.Extension,

                                    };

            response.Items.AddRange(BusinessGalleryes.ToArray());
            return await Task.FromResult(response);

        }
        public override async Task<BusinessGalleryListGm> GetByUserId(BusinessGalleryQueryFilter request, ServerCallContext context)
        {
            BusinessGalleryListGm response = new BusinessGalleryListGm();

            IQueryable<BusinessGallery> query = _businessGalleryRepository.GetQuery();

            query = query.Where(x => x.CreatedById == getUserId(context));

            response.CurrentPage = request.PageId;
            response.ItemCount = await _businessGalleryRepository.GetCount(query);
            response.PageCount = (response.ItemCount / request.PageSize) + 1;



            var BusinessGalleryes = from BusinessGallery in await _businessGalleryRepository.GetAll(query, request.PageId, request.PageSize)
                                    select new BusinessGalleryGm()
                                    {
                                        Id = BusinessGallery.BusinessGalleryId.ToString(),
                                        Name = BusinessGallery.Name,
                                        Extension = BusinessGallery.Extension,
                                        Url = BusinessGallery.Name + BusinessGallery.Extension,

                                    };

            response.Items.AddRange(BusinessGalleryes.ToArray());
            return await Task.FromResult(response);

        }
        public override async Task<BusinessGalleryGm> GetById(BusinessGalleryIdFilter request, ServerCallContext context)
        {
            var BusinessGallery = await _businessGalleryRepository.Find(request.BusinessGalleryId);
            var searchedBusinessGallery = new BusinessGalleryGm()
            {
                Id = BusinessGallery.BusinessGalleryId.ToString(),
                 Name = BusinessGallery.Name,
                 Extension = BusinessGallery.Extension,
                Url = BusinessGallery.Name + BusinessGallery.Extension,

            };
            return await Task.FromResult(searchedBusinessGallery);
        }

        public override async Task<BusinessGalleryGm> Post(BusinessGalleryGm request, ServerCallContext context)
        {



            var BusinessGallery = new BusinessGallery()
            {
                BusinessGalleryId = Guid.NewGuid(),
                Name = request.Name,
                CreatedById = getUserId(context),
                Extension = request.Extension,
                CreatedDate = DateTime.Now,
                Published = true,
                BusinessId = Guid.Parse(request.BusinessId),
            };

            var res = await _businessGalleryRepository.Add(BusinessGallery);

            var response = new BusinessGalleryGm()
            {
                Id = res.BusinessGalleryId.ToString(),

                Name = res.Name,
                Extension = res.Extension,
            };
            return await Task.FromResult(response);
        }


        public override async Task<BusinessGalleryGm> Put(BusinessGalleryGm request,
          ServerCallContext context)
        {
            BusinessGallery BusinessGallery = await _businessGalleryRepository.Find(request.Id);
            if (BusinessGallery == null)
            {
                return await Task.FromResult<BusinessGalleryGm>(null);
            }


            BusinessGallery.Name = request.Name;
            BusinessGallery.Extension = request.Extension;
            BusinessGallery.UpdatedById = getUserId(context);
            BusinessGallery.UpdatedDate = DateTime.Now;




            await _businessGalleryRepository.Update(BusinessGallery);
            return await Task.FromResult<BusinessGalleryGm>(new BusinessGalleryGm()
            {
                Id = BusinessGallery.BusinessGalleryId.ToString(),
                Name = BusinessGallery.Name,
                Extension = BusinessGallery.Extension,
            });
        }
        public override async Task<Empty> Delete(BusinessGalleryIdFilter request, ServerCallContext context)
        {
            BusinessGallery BusinessGallery = await _businessGalleryRepository.Find(request.BusinessGalleryId);
            if (BusinessGallery == null)
            {
                return await Task.FromResult<Empty>(null);
            }

            await _businessGalleryRepository.Delete(BusinessGallery);
            return await Task.FromResult<Empty>(new Empty());
        }

        public override async Task<Empty> SoftDelete(BusinessGalleryIdFilter request, ServerCallContext context)
        {
            BusinessGallery BusinessGallery = await _businessGalleryRepository.Find(request.BusinessGalleryId);
            
            if (BusinessGallery == null)
            {
                return await Task.FromResult<Empty>(null);
            }

            BusinessGallery.Deleted = true;
            BusinessGallery.UpdatedById = getUserId(context);
            BusinessGallery.UpdatedDate = DateTime.Now;

            await _businessGalleryRepository.Update(BusinessGallery);
            return await Task.FromResult<Empty>(new Empty());
        }
        public override async Task<Google.Protobuf.WellKnownTypes.Empty> SoftDeleteCollection(BusinessGalleryDeleteCollectionGm request, ServerCallContext context)
        {

            foreach (var item in request.Items)
            {
                BusinessGallery BusinessGallery = await _businessGalleryRepository.Find(item.BusinessGalleryId);

                if (BusinessGallery == null)
                {
                    return await Task.FromResult<Empty>(null);
                }

                BusinessGallery.Deleted = true;
                BusinessGallery.UpdatedById = getUserId(context);
                BusinessGallery.UpdatedDate = DateTime.Now;

                await _businessGalleryRepository.Update(BusinessGallery);
            }

            return await Task.FromResult<Empty>(new Empty());
        }
    }
}
