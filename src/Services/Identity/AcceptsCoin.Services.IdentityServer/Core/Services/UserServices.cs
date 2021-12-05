using AcceptsCoin.Common.Auth;
using AcceptsCoin.Services.IdentityServer.Core.Interfaces;
using AcceptsCoin.Services.IdentityServer.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcceptsCoin.Services.IdentityServer.Core.Services
{
    public class UserServices : IUserServices
    {

        private readonly IUserRepository _userRepository;
        private readonly IJwtHandler _jwtHandler;
        public UserServices(IUserRepository userRepository, IJwtHandler jwtHandler)
        {
            _userRepository = userRepository;
            _jwtHandler = jwtHandler;
        }
        public async Task<JsonWebToken> LoginAsync(string userName, string password)
        {
            var user = await _userRepository.Find(userName);
            if (user == null)
            {
                // throw new ActioExeption("invalid_credentials",
                //    $"Invalid User");
            }


            return _jwtHandler.Create(user.UserId);
        }
    }
}
