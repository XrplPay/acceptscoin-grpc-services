using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.Services.DirectoryServer.Domain.Models;

namespace AcceptsCoin.Services.DirectoryServer.Domain.Interfaces
{
   
    public interface ITagRepository
    {

        Task<Tag> Find(string Id);

        Task<Tag> Add(Tag entity);

        IQueryable<Tag> GetQuery();

        Task<int> GetCount(IQueryable<Tag> query);

        Task<IEnumerable<Tag>> GetAll(IQueryable<Tag> query, int pageId, int pageSize);

        Task<IEnumerable<Tag>> GetAll();


        Task<Tag> Update(Tag entity);

        Task Delete(Tag entity);
    }
}
