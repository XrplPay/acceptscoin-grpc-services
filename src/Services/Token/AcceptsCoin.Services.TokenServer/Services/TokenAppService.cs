using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.Services.TokenServer.Core.Interfaces;
using AcceptsCoin.Services.TokenServer.Domain.Interfaces;
using AcceptsCoin.Services.TokenServer.Domain.Models;
using AcceptsCoin.Services.TokenServer.Protos;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace AcceptsCoin.Services.TokenServer
{
    //[Authorize]
    public class TokenGrpcService : TokenAppService.TokenAppServiceBase
    {
        private readonly ILogger<TokenGrpcService> _logger;
        private ITokenService _tokenService;
        public TokenGrpcService(ILogger<TokenGrpcService> logger, ITokenService tokenService)
        {
            _logger = logger;
            _tokenService = tokenService;
        }

        public override async Task<TokenListGm> GetAll(Empty request, ServerCallContext context)
        {
            TokenListGm response = new TokenListGm();

            Console.WriteLine(context.GetHttpContext().User.Identity.Name);
            var categories = from prd in await _tokenService.GetAll()
            select new TokenGm()
            {
                TokenId = prd.TokenId.ToString(),
                Description=prd.Description,
                Link=prd.Link,
                Symbol=prd.Symbol,
                Icon = prd.Icon,
                Logo = prd.Logo,
                Name = prd.Name,
                Priority = prd.Priority,
            };




            response.Items.AddRange(categories.ToArray());
            return await Task.FromResult(response);

        }
        public override async Task<TokenGm> GetById(TokenIdFilter request,ServerCallContext context)
        {
            var Token =await _tokenService.Find(Guid.Parse(request.TokenId));
            var searchedToken = new TokenGm()
            {
               TokenId=Token.TokenId.ToString(),
               Icon=Token.Icon,
               Name=Token.Name,
               Logo=Token.Logo,
               Priority=Token.Priority,
               Description=Token.Description,
               Link=Token.Link,
               Symbol=Token.Symbol,
            };
            return await Task.FromResult(searchedToken);
        }

        public override async Task<TokenGm> Post(TokenGm request, ServerCallContext context)
        {
          
            var prdAdded = new Token()
            {
                TokenId = Guid.NewGuid(),
                Name = request.Name,
                Icon = request.Icon,
                Logo = request.Logo,
                Priority = request.Priority,
                CreatedById = Guid.Parse("999bb90f-3167-4f81-83bb-0c76d1d3ace5"),
                CreatedDate = DateTime.Now,
                Published = true,
                Symbol=request.Symbol,
                Link=request.Link,
                Description=request.Description,
                
            };

            var res = await _tokenService.Add(prdAdded);

            var response = new TokenGm()
            {
                TokenId = res.TokenId.ToString(),
                Name = res.Name,
                Icon = res.Icon,
                Logo = res.Logo,
                Priority = res.Priority,
                Description=res.Description,
                Link=res.Link,
                Symbol=res.Symbol,

            };
            return await Task.FromResult(response);
        }


        public override async Task<TokenGm> Put(TokenGm request,
           ServerCallContext context)
        {
            Token prd = await _tokenService.Find(Guid.Parse(request.TokenId));
            if (prd == null)
            {
                return await Task.FromResult<TokenGm>(null);
            }


            prd.Name = request.Name;
            prd.Logo = request.Logo;
            prd.Icon = request.Icon;
            prd.Priority = request.Priority;
            prd.UpdatedById = Guid.Parse("999bb90f-3167-4f81-83bb-0c76d1d3ace5");
            prd.UpdatedDate = DateTime.Now;
            prd.Symbol = request.Symbol;
            prd.Icon = request.Icon;
            prd.Link = request.Link;





            await _tokenService.Update(prd);
            return await Task.FromResult<TokenGm>(new TokenGm()
            {
                TokenId = prd.TokenId.ToString(),
                Icon = prd.Icon,
                Logo = prd.Logo,
                Name = prd.Name,
                Priority = prd.Priority,
                Link=prd.Link,
                Symbol=prd.Symbol,
                Description=prd.Description,
            });
        }

        public override async Task<Empty> Delete(TokenIdFilter request, ServerCallContext context)
        {
            Token prd = await _tokenService.Find(Guid.Parse(request.TokenId));
            if (prd == null)
            {
                return await Task.FromResult<Empty>(null);
            }

            await _tokenService.Delete(prd);
            return await Task.FromResult<Empty>(new Empty());
        }
    }
}
