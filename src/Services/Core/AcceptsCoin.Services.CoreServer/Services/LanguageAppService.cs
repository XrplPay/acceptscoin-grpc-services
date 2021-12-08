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
    public class LanguageGrpcService : LanguageAppService.LanguageAppServiceBase
    {
        private readonly ILogger<LanguageGrpcService> _logger;
        private ILanguageService _languageService;
        public LanguageGrpcService(ILogger<LanguageGrpcService> logger, ILanguageService languageService)
        {
            _logger = logger;
            _languageService = languageService;
        }

        public override async Task<LanguageListGm> GetAll(EmptyLanguage request, ServerCallContext context)
        {
            LanguageListGm response = new LanguageListGm();

            Console.WriteLine(context.GetHttpContext().User.Identity.Name);
            var categories = from prd in await _languageService.GetAll()
            select new LanguageGm()
            {
                LanguageId = prd.LanguageId.ToString(),
                Name = prd.Name,
                Code = prd.Code,
                Logo = prd.Logo,
                Icon = prd.Icon,
                Priority = prd.Priority,                                 
            };
            response.Items.AddRange(categories.ToArray());
            return await Task.FromResult(response);
        }
     
        public override async Task<LanguageGm> GetById(LanguageIdFilter request,ServerCallContext context)
        {
            var Language =await _languageService.Find(Guid.Parse(request.LanguageId));
            var searchedLanguage = new LanguageGm()
            {
               LanguageId=Language.LanguageId.ToString(),
               Icon=Language.Icon,
               Name=Language.Name,
               Logo=Language.Logo,
               Priority=Language.Priority,
               Code=Language.Code,
            };
            return await Task.FromResult(searchedLanguage);
        }

        public override async Task<LanguageGm> Post(LanguageGm request, ServerCallContext context)
        {

            var prdAdded = new Language()
            {
                LanguageId = Guid.NewGuid(),
                Name = request.Name,
                Icon = request.Icon,
                Logo = request.Logo,
                Priority = request.Priority,
                CreatedById = Guid.Parse("999bb90f-3167-4f81-83bb-0c76d1d3ace5"),
                CreatedDate = DateTime.Now,
                Published = true,
                Code = request.Code,
                Deleted = false,


            };

            var res = await _languageService.Add(prdAdded);

            var response = new LanguageGm()
            {
                LanguageId = res.LanguageId.ToString(),
                Name = res.Name,
                Icon = res.Icon,
                Logo = res.Logo,
                Priority = res.Priority,
                Code = res.Code,
                

            };
            return await Task.FromResult(response);
        }


        public override async Task<LanguageGm> Put(LanguageGm request,
           ServerCallContext context)
        {
            Language prd = await _languageService.Find(Guid.Parse(request.LanguageId));
            if (prd == null)
            {
                return await Task.FromResult<LanguageGm>(null);
            }


            prd.Name = request.Name;
            prd.Logo = request.Logo;
            prd.Icon = request.Icon;
            prd.Priority = request.Priority;
            prd.UpdatedById = Guid.Parse("999bb90f-3167-4f81-83bb-0c76d1d3ace5");
            prd.UpdatedDate = DateTime.Now;
            prd.Code = request.Code;
            




            await _languageService.Update(prd);
            return await Task.FromResult<LanguageGm>(new LanguageGm()
            {
                LanguageId = prd.LanguageId.ToString(),
                Icon = prd.Icon,
                Logo = prd.Logo,
                Name = prd.Name,
                Priority = prd.Priority,
                Code = prd.Code,
            });
        }


        
        public override async Task<EmptyLanguage> Delete(LanguageIdFilter request, ServerCallContext context)
        {
            Language prd = await _languageService.Find(Guid.Parse(request.LanguageId));
            if (prd == null)
            {
                return await Task.FromResult<EmptyLanguage>(null);
            }

            await _languageService.Delete(prd);
            return await Task.FromResult<EmptyLanguage>(new EmptyLanguage());
        }
    }
}
