using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.Services.DirectoryServer.Domain.Models;

namespace AcceptsCoin.Services.DirectoryServer.Domain.Interfaces
{
    

    public interface IBusinessTokenRepository
    {
        IQueryable<BusinessToken> GetQuery();

        Task<int> GetCount(IQueryable<BusinessToken> query);

        Task<IEnumerable<BusinessToken>> GetAll(IQueryable<BusinessToken> query, int pageId, int pageSize);

        Task<IEnumerable<BusinessToken>> GetAll();

        Task<BusinessToken> Find(Guid BusinessId, Guid TokenId);


        Task<BusinessToken> Add(BusinessToken entity);

        Task<BusinessToken> Update(BusinessToken entity);

        Task Delete(BusinessToken entity);
    }
}
