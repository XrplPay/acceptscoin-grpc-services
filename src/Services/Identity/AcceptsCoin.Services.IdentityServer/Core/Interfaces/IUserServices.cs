using AcceptsCoin.Common.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcceptsCoin.Services.IdentityServer.Core.Interfaces
{
    public interface IUserServices
    {
        Task<JsonWebToken> LoginAsync(string email, string password);
    }
}
