using System;
using System.Net;
using System.Threading.Tasks;
using AcceptsCoin.Services.DirectoryServer.Domain.Interfaces;
using AcceptsCoin.Services.DirectoryServer.Domain.Models;
using AcceptsCoin.Services.DirectoryServer.Protos;
using Grpc.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace AcceptsCoin.Services.DirectoryServer.Services
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DirectoryGrpcService : DirectoryAppService.DirectoryAppServiceBase
    {
        private readonly ILogger<DirectoryGrpcService> _logger;
        private IUserRepository _userRepository;
        private ITokenRepository _tokenRepository;
        public DirectoryGrpcService(ILogger<DirectoryGrpcService> logger, IUserRepository userRepository, ITokenRepository tokenRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
        }

        public override async Task<DirectoryUserGm> DirectoryUserPost(DirectoryUserGm request, ServerCallContext context)
        {
            var user = new User()
            {
                UserId = Guid.Parse(request.Id),
                Name = request.Name,
                Email = request.Email,
            };

            var res = await _userRepository.Add(user);

            var response = new DirectoryUserGm()
            {
                Id = res.UserId.ToString(),
                Email = res.Email,
                Name = res.Name,
            };
            return await Task.FromResult(response);
        }


        public override async Task<DirectoryUserGm> DirectoryUserPut(DirectoryUserGm request,
          ServerCallContext context)
        {
            User user = await _userRepository.Find(Guid.Parse(request.Id));
            if (user == null)
            {
                return await Task.FromResult<DirectoryUserGm>(null);
            }
            user.Email = request.Email;
            user.Name = request.Name;
            
            await _userRepository.Update(user);
            return await Task.FromResult<DirectoryUserGm>(new DirectoryUserGm()
            {
                Id = user.UserId.ToString(),
                Email = user.Email,
                Name = user.Name,
            });
        }



        public override async Task<DirectoryTokenGm> DirectoryTokenPost(DirectoryTokenGm request, ServerCallContext context)
        {
            var token = new Token()
            {
                TokenId = Guid.Parse(request.Id),
                Name = request.Name,
                Icon = request.Icon,
                Logo = request.Logo,
                Symbol = request.Symbol,
            };

            var res = await _tokenRepository.Add(token);

            var response = new DirectoryTokenGm()
            {
                Id = res.TokenId.ToString(),
                Icon = res.Icon,
                Name = res.Name,
                Symbol = res.Symbol,
                Logo = res.Logo,
            };
            return await Task.FromResult(response);
        }


        public override async Task<DirectoryTokenGm> DirectoryTokenPut(DirectoryTokenGm request,
          ServerCallContext context)
        {
             Token token = await _tokenRepository.Find(Guid.Parse(request.Id));
            if (token == null)
            {
                return await Task.FromResult<DirectoryTokenGm>(null);
            }
            token.Symbol = request.Symbol;
            token.Name = request.Name;
            token.Logo = request.Logo;
            token.Icon = request.Icon;

            await _tokenRepository.Update(token);
            return await Task.FromResult<DirectoryTokenGm>(new DirectoryTokenGm()
            {
                Id = token.TokenId.ToString(),
                Icon = token.Icon,
                Name = token.Name,
                Symbol = token.Symbol,
                Logo = token.Logo,

            });
        }
    }

}