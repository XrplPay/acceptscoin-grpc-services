using System;
using System.Threading.Tasks;
using AcceptsCoin.Services.IdentityServer.Domain.Models;

namespace AcceptsCoin.Services.IdentityServer.Domain.Interfaces
{
   
    public interface IPartnerRepository
    {

        Task<Partner> Find(string Id);

        Task<Partner> Add(Partner entity);

    }
}
