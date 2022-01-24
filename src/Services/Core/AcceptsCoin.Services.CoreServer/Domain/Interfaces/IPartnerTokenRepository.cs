using AcceptsCoin.Services.CoreServer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcceptsCoin.Services.CoreServer.Domain.Interfaces
{
    public interface IPartnerTokenRepository
    {
        IQueryable<PartnerToken> GetQuery();

        Task<int> GetCount(IQueryable<PartnerToken> query);

        Task<IEnumerable<PartnerToken>> GetAll(IQueryable<PartnerToken> query, int pageId, int pageSize);

        Task<IEnumerable<PartnerToken>> GetAll();

        Task<PartnerToken> Find(Guid TokenId, Guid PartnerId);


        Task<PartnerToken> Add(PartnerToken entity);

        Task<PartnerToken> Update(PartnerToken entity);

        Task Delete(PartnerToken entity);
    }
}
