using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.Services.CoreServer.Domain;
using AcceptsCoin.Services.CoreServer.Domain.Models;

namespace AcceptsCoin.Services.CoreServer.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        IQueryable<Category> GetQuery();

        Task<int> GetCount(IQueryable<Category> query);

        Task<IEnumerable<Category>> GetAll(IQueryable<Category> query, int pageId, int pageSize);

        Task<IEnumerable<Category>> GetAll();

        Task<Category> Find(string Id);

        Task<Category> Add(Category entity);

        Task<Category> Update(Category entity);

        Task Delete(Category entity);
    }
}
