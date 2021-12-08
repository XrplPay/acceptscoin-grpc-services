using AcceptsCoin.Services.CoreServer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcceptsCoin.Services.CoreServer.Core.Interfaces
{
    public interface IPartnerService
    {
        Task<IEnumerable<Partner>> GetAll();
        Task<Partner> Find(Guid Id);

        Task<Partner> Add(Partner entity);

        Task<Partner> Update(Partner entity);

        Task Delete(Partner entity);

        Task SoftDelete(Partner entity);
    }
}
