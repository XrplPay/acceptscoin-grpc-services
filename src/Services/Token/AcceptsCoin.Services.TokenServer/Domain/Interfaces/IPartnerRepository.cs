using System;
using System.Threading.Tasks;
using AcceptsCoin.Services.TokenServer.Domain.Models;

namespace AcceptsCoin.Services.TokenServer.Domain.Interfaces
{
   
    public interface IPartnerRepository
    {

        Task<Partner> Find(string Id);

        Task<Partner> Add(Partner entity);

    }
}
