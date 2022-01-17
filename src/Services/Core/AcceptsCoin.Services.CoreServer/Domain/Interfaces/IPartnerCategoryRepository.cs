using AcceptsCoin.Services.CoreServer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcceptsCoin.Services.CoreServer.Domain.Interfaces
{
    public interface IPartnerCategoryRepository
    {
        IQueryable<PartnerCategory> GetQuery();

        Task<int> GetCount(IQueryable<PartnerCategory> query);

        Task<IEnumerable<PartnerCategory>> GetAll(IQueryable<PartnerCategory> query, int pageId, int pageSize);

        Task<IEnumerable<PartnerCategory>> GetAll();

        Task<PartnerCategory> Find(Guid CategoryId, Guid PartnerId);


        Task<PartnerCategory> Add(PartnerCategory entity);

        Task<PartnerCategory> Update(PartnerCategory entity);

        Task Delete(PartnerCategory entity);
    }
}
