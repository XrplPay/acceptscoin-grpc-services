using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.Services.PosServer.Domain.Models;

namespace AcceptsCoin.Services.PosServer.Domain.Interfaces
{
    public interface IStoreRepository
    {
        IQueryable<Store> GetQuery();

        Task<int> GetCount(IQueryable<Store> query);

        Task<IEnumerable<Store>> GetAll();

        Task<IEnumerable<Store>> GetAll(IQueryable<Store> query, int pageId, int pageSize);

        Task<Store> Find(string Id);

        Task<Store> FindByEmail(string email);

        Task<Store> FindByWebSiteUrl(string webSiteUrl);

        Task<Store> Find(string email,string webSiteUrl);

        Task<Store> Add(Store entity);

        Task<Store> Update(Store entity);

        Task Delete(Store entity);
    }
}
