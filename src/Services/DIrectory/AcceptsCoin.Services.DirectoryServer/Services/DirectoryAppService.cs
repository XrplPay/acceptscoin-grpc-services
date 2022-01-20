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
        public DirectoryGrpcService(ILogger<DirectoryGrpcService> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
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
    }

}