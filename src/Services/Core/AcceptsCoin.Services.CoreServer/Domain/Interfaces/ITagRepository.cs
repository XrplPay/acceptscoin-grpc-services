using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.Services.CoreServer.Domain;
using AcceptsCoin.Services.CoreServer.Domain.Models;

namespace AcceptsCoin.Services.CoreServer.Domain.Interfaces
{
    public interface ITagRepository
    {
        IQueryable<Tag> GetQuery();

        Task<int> GetCount(IQueryable<Tag> query);

        Task<IEnumerable<Tag>> GetAll(IQueryable<Tag> query, int pageId, int pageSize);

        Task<IEnumerable<Tag>> GetAll();
        Task<Tag> Find(Guid Id);
        Task<Tag> Add(Tag entity);

        Task<Tag> Update(Tag entity);

        Task Delete(Tag entity);
    }
}
