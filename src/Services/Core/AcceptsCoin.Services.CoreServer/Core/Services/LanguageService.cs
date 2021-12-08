using AcceptsCoin.Services.CoreServer.Core.Interfaces;
using AcceptsCoin.Services.CoreServer.Domain.Interfaces;
using AcceptsCoin.Services.CoreServer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcceptsCoin.Services.CoreServer.Core.Services
{
    public class LanguageService : ILanguageService
    {

        private readonly ILanguageRepository _LanguageRepository;
        public LanguageService(ILanguageRepository LanguageRepository)
        {
            _LanguageRepository = LanguageRepository;
        }

        public async Task<Language> Add(Language entity)
        {
            return await _LanguageRepository.Add(entity);
        }

        public async Task Delete(Language entity)
        {
            await _LanguageRepository.Delete(entity);
        }

        public async Task<Language> Find(Guid Id)
        {
            return await _LanguageRepository.Find(Id);
        }

        public async Task<IEnumerable<Language>> GetAll()
        {
            return await _LanguageRepository.GetAll();
        }

        public async Task SoftDelete(Language entity)
        {
            entity.Deleted = true;
            await _LanguageRepository.Update(entity);
        }

        public async Task<Language> Update(Language entity)
        {
            return await _LanguageRepository.Update(entity);
        }
    }
}
