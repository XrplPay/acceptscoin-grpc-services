using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.Common.Auth;
using AcceptsCoin.Services.IdentityServer.Core.Interfaces;
using AcceptsCoin.Services.IdentityServer.Domain.Interfaces;
using AcceptsCoin.Services.IdentityServer.Domain.Models;
using AcceptsCoin.Services.IdentityServer.Protos;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;


namespace AcceptsCoin.Services.IdentityServer.Services
{
    public class AuthService : AuthAppService.AuthAppServiceBase
    {
        private readonly ILogger<AuthService> _logger;
        private IUserServices _userServices;
        public AuthService(ILogger<AuthService> logger, IUserServices userServices)
        {
            _logger = logger;
            _userServices = userServices;
        }

        public override async Task<TokenGm> Authenticate(AuthGm request,
           ServerCallContext context)
        {
            JsonWebToken prd = await _userServices.LoginAsync(request.Username,request.Password);
            if (prd == null)
            {
                return await Task.FromResult<TokenGm>(null);
            }

            return await Task.FromResult<TokenGm>(new TokenGm()
            {
                Token = prd.Token,
                Expires = prd.Expires,
            });
        }

      
    }
}
