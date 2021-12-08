using System;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.Services.CoreServer.Core.Interfaces;
using AcceptsCoin.Services.CoreServer.Domain.Models;
using AcceptsCoin.Services.CoreServer.Protos;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace AcceptsCoin.Services.CoreServer
{
    //[Authorize]
    public class PartnerGrpcService : PartnerAppService.PartnerAppServiceBase
    {
        private readonly ILogger<PartnerGrpcService> _logger;
        private IPartnerService _tagService;
        public PartnerGrpcService(ILogger<PartnerGrpcService> logger, IPartnerService tagService)
        {
            _logger = logger;
            _tagService = tagService;
        }

        public override async Task<PartnerListGm> GetAll(EmptyPartner request, ServerCallContext context)
        {
            PartnerListGm response = new PartnerListGm();

            Console.WriteLine(context.GetHttpContext().User.Identity.Name);
            var categories = from prd in await _tagService.GetAll()
            select new PartnerGm()
            {
                PartnerId = prd.PartnerId.ToString(),
                Name = prd.Name,
                Logo = prd.Logo,
                WebSiteUrl = prd.WebSiteUrl,
                Email = prd.Email,
                ContactNumber = prd.ContactNumber,
                Manager = prd.Manager,
                Owner = prd.Owner,
                LanguageId = prd.LanguageId.ToString(),
                                        
            };
            response.Items.AddRange(categories.ToArray());
            return await Task.FromResult(response);
        }

        public override async Task<PartnerGm> GetById(PartnerIdFilter request,ServerCallContext context)
        {
            var Partner =await _tagService.Find(Guid.Parse(request.PartnerId));
            var searchedPartner = new PartnerGm()
            {
               PartnerId=Partner.PartnerId.ToString(),
                Name = Partner.Name,
                Logo = Partner.Logo,
                WebSiteUrl = Partner.WebSiteUrl,
                Email = Partner.Email,
                ContactNumber = Partner.ContactNumber,
                Manager = Partner.Manager,
                Owner = Partner.Owner,
                LanguageId = Partner.LanguageId.ToString(),

            };
            return await Task.FromResult(searchedPartner);
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
                CreatedById = Guid.Parse("999bb90f-3167-4f81-83bb-0c76d1d3ace5"),
                CreatedDate = DateTime.Now,
                Published = true,
                Deleted = false,
            };

            var res = await _tagService.Add(prdAdded);

            var response = new PartnerGm()
            {
                PartnerId = res.PartnerId.ToString(),
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
            Partner prd = await _tagService.Find(Guid.Parse(request.PartnerId));
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
            prd.UpdatedById = Guid.Parse("999bb90f-3167-4f81-83bb-0c76d1d3ace5");
            prd.UpdatedDate = DateTime.Now;
            




            await _tagService.Update(prd);
            return await Task.FromResult<PartnerGm>(new PartnerGm()
            {
                PartnerId = prd.PartnerId.ToString(),
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
            Partner prd = await _tagService.Find(Guid.Parse(request.PartnerId));
            if (prd == null)
            {
                return await Task.FromResult<EmptyPartner>(null);
            }

            await _tagService.Delete(prd);
            return await Task.FromResult<EmptyPartner>(new EmptyPartner());
        }
    }
}
