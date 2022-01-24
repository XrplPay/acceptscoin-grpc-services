using System;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.Services.CoreServer.Core.Interfaces;
using AcceptsCoin.Services.CoreServer.Domain.Interfaces;
using AcceptsCoin.Services.CoreServer.Domain.Models;
using AcceptsCoin.Services.CoreServer.Protos;
using Grpc.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace AcceptsCoin.Services.CoreServer
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PartnerGrpcService : PartnerAppService.PartnerAppServiceBase
    {
        private readonly ILogger<PartnerGrpcService> _logger;
        private IPartnerRepository  _partnerRepository;
        public PartnerGrpcService(ILogger<PartnerGrpcService> logger, IPartnerRepository partnerRepository)
        {
            _logger = logger;
            _partnerRepository = partnerRepository;
        }

        private Guid getUserId(ServerCallContext context)
        {
            return Guid.Parse(context.GetHttpContext().User.Identity.Name);
        }
        private string getPartnetId(ServerCallContext context)
        {
            return "bff3b2dd-e89d-46fc-a868-aab93a3efbbe";
        }
        public override async Task<PartnerListGm> GetAll(PartnerQueryFilter request, ServerCallContext context)
        {
            PartnerListGm response = new PartnerListGm();

            IQueryable<Partner> query = _partnerRepository.GetQuery();


            response.CurrentPage = request.PageId;
            response.ItemCount = await _partnerRepository.GetCount(query);
            response.PageCount = (response.ItemCount / request.PageSize) + 1;


            var partners = from partner in await _partnerRepository.GetAll(query, request.PageId, request.PageSize)
                           select new PartnerGm()
                           {
                               Id = partner.PartnerId.ToString(),
                               Name = partner.Name,
                               Logo = partner.Logo,
                               WebSiteUrl = partner.WebSiteUrl,
                               Email = partner.Email,
                               ContactNumber = partner.ContactNumber,
                               Manager = partner.Manager,
                               Owner = partner.Owner,
                               ApiKey = partner.ApiKey.ToString(),
                               LanguageId = partner.LanguageId.ToString(),
                           };
            response.Items.AddRange(partners.ToArray());
            return await Task.FromResult(response);
        }

        public override async Task<PartnerListGm> GetByTokenId(PartnerTokenQueryFilter request, ServerCallContext context)
        {
            PartnerListGm response = new PartnerListGm();

            IQueryable<Partner> query = _partnerRepository.GetQuery();
            query = query.Where(x => x.PartnerTokens.Any(z => z.TokenId == Guid.Parse(request.TokenId)));

            response.CurrentPage = request.PageId;
            response.ItemCount = await _partnerRepository.GetCount(query);
            response.PageCount = (response.ItemCount / request.PageSize) + 1;


            var partners = from partner in await _partnerRepository.GetAll(query, request.PageId, request.PageSize)
                           select new PartnerGm()
                           {
                               Id = partner.PartnerId.ToString(),
                               Name = partner.Name,
                               Logo = partner.Logo,
                               WebSiteUrl = partner.WebSiteUrl,
                               Email = partner.Email,
                               ContactNumber = partner.ContactNumber,
                               Manager = partner.Manager,
                               Owner = partner.Owner,
                               ApiKey = partner.ApiKey.ToString(),
                               LanguageId = partner.LanguageId.ToString(),
                           };
            response.Items.AddRange(partners.ToArray());
            return await Task.FromResult(response);
        }

        public override async Task<PartnerGm> GetById(PartnerIdFilter request,ServerCallContext context)
        {
            var partner =await _partnerRepository.Find(Guid.Parse(request.PartnerId));
            var searchedPartner = new PartnerGm()
            {
               Id= partner.PartnerId.ToString(),
                Name = partner.Name,
                Logo = partner.Logo,
                WebSiteUrl = partner.WebSiteUrl,
                Email = partner.Email,
                ContactNumber = partner.ContactNumber,
                Manager = partner.Manager,
                Owner = partner.Owner,
                LanguageId = partner.LanguageId.ToString(),

            };
            return await Task.FromResult(searchedPartner);
        }


        [AllowAnonymous]
        public override async Task<PartnerIdFilter> GetPartnerIdByApiKey(PartnerApiKeyGm request, ServerCallContext context)
        {
            PartnerIdFilter response = new PartnerIdFilter();
            var partner = await _partnerRepository.FindByApiKey(Guid.Parse(request.ApiKey));
            if (partner != null)
            {
                response.PartnerId = partner.PartnerId.ToString();
            }
            return await Task.FromResult(response);
        }

        public override async Task<PartnerGm> Post(PartnerGm request, ServerCallContext context)
        {

            var prdAdded = new Partner()
            {
                PartnerId = Guid.NewGuid(),
                Name = request.Name,
                Logo = request.Logo,
                WebSiteUrl = request.WebSiteUrl,
                Email = request.Email,
                ContactNumber = request.ContactNumber,
                Manager = request.Manager,
                Owner = request.Owner,
                LanguageId = Guid.Parse(request.LanguageId),
                CreatedById = getUserId(context),
                CreatedDate = DateTime.Now,
                Published = true,
                Deleted = false,
                ApiKey = Guid.NewGuid(),
            };

            var res = await _partnerRepository.Add(prdAdded);

            var response = new PartnerGm()
            {
                Id = res.PartnerId.ToString(),
                Name = res.Name,
                Logo = res.Logo,
                WebSiteUrl = res.WebSiteUrl,
                Email = res.Email,
                ContactNumber = res.ContactNumber,
                Manager = res.Manager,
                Owner = res.Owner,
                LanguageId = res.LanguageId.ToString(),
            };
            return await Task.FromResult(response);
        }


        public override async Task<PartnerGm> Put(PartnerGm request,
           ServerCallContext context)
        {
            Partner prd = await _partnerRepository.Find(Guid.Parse(request.Id));
            if (prd == null)
            {
                return await Task.FromResult<PartnerGm>(null);
            }

            prd.Name = request.Name;
            prd.Logo = request.Logo;
            prd.WebSiteUrl = request.WebSiteUrl;
            prd.Email = request.Email;
            prd.ContactNumber = request.ContactNumber;
            prd.Manager = request.Manager;
            prd.Owner = request.Owner;
            prd.LanguageId = Guid.Parse(request.LanguageId);
            prd.UpdatedById = getUserId(context);
            prd.UpdatedDate = DateTime.Now;
            




            await _partnerRepository.Update(prd);
            return await Task.FromResult<PartnerGm>(new PartnerGm()
            {
                Id = prd.PartnerId.ToString(),
                Name = prd.Name,
                Logo = prd.Logo,
                WebSiteUrl = prd.WebSiteUrl,
                Email = prd.Email,
                ContactNumber = prd.ContactNumber,
                Manager = prd.Manager,
                Owner = prd.Owner,
                LanguageId = prd.LanguageId.ToString(),
            });
        }


        
        public override async Task<EmptyPartner> Delete(PartnerIdFilter request, ServerCallContext context)
        {
            Partner prd = await _partnerRepository.Find(Guid.Parse(request.PartnerId));
            if (prd == null)
            {
                return await Task.FromResult<EmptyPartner>(null);
            }

            await _partnerRepository.Delete(prd);
            return await Task.FromResult<EmptyPartner>(new EmptyPartner());
        }

        public override async Task<EmptyPartner> SoftDelete(PartnerIdFilter request, ServerCallContext context)
        {
            Partner partner = await _partnerRepository.Find(Guid.Parse(request.PartnerId));

            if (partner == null)
            {
                return await Task.FromResult<EmptyPartner>(null);
            }

            partner.Deleted = true;
            partner.UpdatedById = getUserId(context);
            partner.UpdatedDate = DateTime.Now;

            await _partnerRepository.Update(partner);
            return await Task.FromResult<EmptyPartner>(new EmptyPartner());
        }
        public override async Task<EmptyPartner> SoftDeleteCollection(PartnerDeleteCollectionGm request, ServerCallContext context)
        {

            foreach (var item in request.Items)
            {
                Partner partner = await _partnerRepository.Find(Guid.Parse(item.PartnerId));

                if (partner == null)
                {
                    return await Task.FromResult<EmptyPartner>(null);
                }

                partner.Deleted = true;
                partner.UpdatedById = getUserId(context);
                partner.UpdatedDate = DateTime.Now;

                await _partnerRepository.Update(partner);
            }

            return await Task.FromResult<EmptyPartner>(new EmptyPartner());
        }
    }
}
