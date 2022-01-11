using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.Services.CoreServer.Domain;
using AcceptsCoin.Services.CoreServer.Domain.Models;

namespace AcceptsCoin.Services.CoreServer.Domain.Interfaces
{
    public interface ILanguageRepository
    {
        IQueryable<Language> GetQuery();

        Task<int> GetCount(IQueryable<Language> query);

        Task<IEnumerable<Language>> GetAll(IQueryable<Language> query, int pageId, int pageSize);

        Task<IEnumerable<Language>> GetAll();

        Task<Language> Find(Guid Id);

        Task<Language> Add(Language entity);

        Task<Language> Update(Language entity);

        Task Delete(Language entity);
    }
}
