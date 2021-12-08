using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AcceptsCoin.Services.CoreServer.Domain;
using AcceptsCoin.Services.CoreServer.Domain.Models;

namespace AcceptsCoin.Services.CoreServer.Domain.Interfaces
{
    public interface IPartnerRepository
    {
        Task<IEnumerable<Partner>> GetAll();
        Task<Partner> Find(Guid Id);
        Task<Partner> Add(Partner entity);

        Task<Partner> Update(Partner entity);

        Task Delete(Partner entity);
    }
}
