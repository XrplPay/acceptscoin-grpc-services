using AcceptsCoin.Common.Auth;
using AcceptsCoin.Services.IdentityServer.Core.Interfaces;
using AcceptsCoin.Services.IdentityServer.Core.Views;
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

        public async Task<UserProfile> GetProfileAsync(Guid userId)
        {
            var user = await _userRepository.Find(userId);
            if (user == null)
            {
                throw new Exception("User Not Found");
            }

            return new UserProfile
            {
                Email = user.Email,
                Name = user.Name,
            };
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
