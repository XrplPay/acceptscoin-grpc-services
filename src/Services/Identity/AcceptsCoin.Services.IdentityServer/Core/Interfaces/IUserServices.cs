using AcceptsCoin.Common.Auth;
using AcceptsCoin.Services.IdentityServer.Core.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcceptsCoin.Services.IdentityServer.Core.Interfaces
{
    public interface IUserServices
    {
        Task<JsonWebToken> LoginAsync(string email, string password);

        Task<UserProfile> GetProfileAsync(Guid userId);
    }
}
