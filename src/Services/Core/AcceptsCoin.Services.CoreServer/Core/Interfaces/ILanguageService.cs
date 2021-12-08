using AcceptsCoin.Services.CoreServer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcceptsCoin.Services.CoreServer.Core.Interfaces
{
    public interface ILanguageService
    {
        Task<IEnumerable<Language>> GetAll();
        Task<Language> Find(Guid Id);

        Task<Language> Add(Language entity);

        Task<Language> Update(Language entity);

        Task Delete(Language entity);

        Task SoftDelete(Language entity);
    }
}
