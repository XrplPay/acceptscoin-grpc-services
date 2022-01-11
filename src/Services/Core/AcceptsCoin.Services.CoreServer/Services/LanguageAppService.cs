using System;
using System.Linq;
using System.Net;
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
    //[Authorize]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class LanguageGrpcService : LanguageAppService.LanguageAppServiceBase
    {
        private readonly ILogger<LanguageGrpcService> _logger;
        private ILanguageRepository  _languageRepository;
        public LanguageGrpcService(ILogger<LanguageGrpcService> logger, ILanguageRepository languageRepository)
        {
            _logger = logger;
            _languageRepository = languageRepository;
        }

        private Guid getUserId(ServerCallContext context)
        {
            return Guid.Parse(context.GetHttpContext().User.Identity.Name);
        }
        private string getPartnetId(ServerCallContext context)
        {
            return "bff3b2dd-e89d-46fc-a868-aab93a3efbbe";
        }
        public override async Task<LanguageListGm> GetAll(LanguageQueryFilter request, ServerCallContext context)
        {
            LanguageListGm response = new LanguageListGm();

            IQueryable<Language> query = _languageRepository.GetQuery();


            response.CurrentPage = request.PageId;
            response.ItemCount = await _languageRepository.GetCount(query);
            response.PageCount = (response.ItemCount / request.PageSize) + 1;

            var languages = from prd in await _languageRepository.GetAll(query, request.PageId, request.PageSize)
            select new LanguageGm()
            {
                Id = prd.LanguageId.ToString(),
                Name = prd.Name,
                Code = prd.Code,
                Logo = prd.Logo,
                Icon = prd.Icon,
                Priority = prd.Priority,                                 
            };
            response.Items.AddRange(languages.ToArray());
            return await Task.FromResult(response);
        }
     
        public override async Task<LanguageGm> GetById(LanguageIdFilter request,ServerCallContext context)
        {
            var Language =await _languageRepository.Find(Guid.Parse(request.LanguageId));
            var searchedLanguage = new LanguageGm()
            {
               Id=Language.LanguageId.ToString(),
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
                CreatedById =getUserId(context),
                CreatedDate = DateTime.Now,
                Published = true,
                Code = request.Code,
                Deleted = false,


            };

            var res = await _languageRepository.Add(prdAdded);

            var response = new LanguageGm()
            {
                Id = res.LanguageId.ToString(),
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
            Language prd = await _languageRepository.Find(Guid.Parse(request.Id));
            if (prd == null)
            {
                return await Task.FromResult<LanguageGm>(null);
            }


            prd.Name = request.Name;
            prd.Logo = request.Logo;
            prd.Icon = request.Icon;
            prd.Priority = request.Priority;
            prd.UpdatedById = getUserId(context);
            prd.UpdatedDate = DateTime.Now;
            prd.Code = request.Code;





            await _languageRepository.Update(prd);
            return await Task.FromResult<LanguageGm>(new LanguageGm()
            {
                Id = prd.LanguageId.ToString(),
                Icon = prd.Icon,
                Logo = prd.Logo,
                Name = prd.Name,
                Priority = prd.Priority,
                Code = prd.Code,
            });
        }


        
        public override async Task<EmptyLanguage> Delete(LanguageIdFilter request, ServerCallContext context)
        {
            Language prd = await _languageRepository.Find(Guid.Parse(request.LanguageId));
            if (prd == null)
            {
                return await Task.FromResult<EmptyLanguage>(null);
            }

            await _languageRepository.Delete(prd);
            return await Task.FromResult<EmptyLanguage>(new EmptyLanguage());
        }

        public override async Task<EmptyLanguage> SoftDelete(LanguageIdFilter request, ServerCallContext context)
        {
            Language  language = await _languageRepository.Find(Guid.Parse(request.LanguageId));

            if (language == null)
            {
                return await Task.FromResult<EmptyLanguage>(null);
            }

            language.Deleted = true;
            language.UpdatedById = getUserId(context);
            language.UpdatedDate = DateTime.Now;

            await _languageRepository.Update(language);
            return await Task.FromResult<EmptyLanguage>(new EmptyLanguage());
        }
        public override async Task<EmptyLanguage> SoftDeleteCollection(LanguageDeleteCollectionGm request, ServerCallContext context)
        {

            foreach (var item in request.Items)
            {
                Language language = await _languageRepository.Find(Guid.Parse(item.LanguageId));

                if (language == null)
                {
                    return await Task.FromResult<EmptyLanguage>(null);
                }

                language.Deleted = true;
                language.UpdatedById = getUserId(context);
                language.UpdatedDate = DateTime.Now;

                await _languageRepository.Update(language);
            }

            return await Task.FromResult<EmptyLanguage>(new EmptyLanguage());
        }
    }
}
