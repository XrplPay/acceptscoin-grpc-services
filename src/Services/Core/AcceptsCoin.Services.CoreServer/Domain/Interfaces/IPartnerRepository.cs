using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.Services.CoreServer.Domain;
using AcceptsCoin.Services.CoreServer.Domain.Models;

namespace AcceptsCoin.Services.CoreServer.Domain.Interfaces
{
    public interface IPartnerRepository
    {
        Task<Partner> FindByApiKey(Guid Apikey);

        IQueryable<Partner> GetQuery();

        Task<int> GetCount(IQueryable<Partner> query);

        Task<IEnumerable<Partner>> GetAll(IQueryable<Partner> query, int pageId, int pageSize);

        Task<IEnumerable<Partner>> GetAll();

        Task<Partner> Find(Guid Id);

        Task<Partner> Add(Partner entity);

        Task<Partner> Update(Partner entity);

        Task Delete(Partner entity);
    }
}
