using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.Services.DirectoryServer.Domain.Models;

namespace AcceptsCoin.Services.DirectoryServer.Domain.Interfaces
{
    

    public interface IBusinessTagRepository
    {
        IQueryable<BusinessTag> GetQuery();

        Task<int> GetCount(IQueryable<BusinessTag> query);

        Task<IEnumerable<BusinessTag>> GetAll(IQueryable<BusinessTag> query, int pageId, int pageSize);

        Task<IEnumerable<BusinessTag>> GetAll();

        Task<BusinessTag> Find(Guid BusinessId, Guid TagId);


        Task<BusinessTag> Add(BusinessTag entity);

        Task<BusinessTag> Update(BusinessTag entity);

        Task Delete(BusinessTag entity);
    }
}
