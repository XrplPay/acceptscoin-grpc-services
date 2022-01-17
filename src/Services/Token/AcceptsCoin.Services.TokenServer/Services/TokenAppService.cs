using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.Services.TokenServer.Core.Interfaces;
using AcceptsCoin.Services.TokenServer.Domain.Interfaces;
using AcceptsCoin.Services.TokenServer.Domain.Models;
using AcceptsCoin.Services.TokenServer.Migrations;
using AcceptsCoin.Services.TokenServer.Protos;
using Grpc.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace AcceptsCoin.Services.TokenServer
{
    //[Authorize]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class TokenGrpcService : TokenAppService.TokenAppServiceBase
    {
        private readonly ILogger<TokenGrpcService> _logger;
        private ITokenService _tokenService;
        private ITokenRepository _tokenRepository;
        private IPartnerTokenRepository _partnerTokenRepository;
        private IPartnerRepository  _partnerRepository;
        public TokenGrpcService(ILogger<TokenGrpcService> logger, ITokenService tokenService, ITokenRepository tokenRepository, IPartnerTokenRepository partnerTokenRepository, IPartnerRepository partnerRepository)
        {
            _logger = logger;
            _tokenService = tokenService;
            _tokenRepository = tokenRepository;
            _partnerTokenRepository = partnerTokenRepository;
            _partnerRepository = partnerRepository;
        }
        private Guid getUserId(ServerCallContext context)
        {
            return Guid.Parse(context.GetHttpContext().User.Identity.Name);
        }
        private string getPartnetId(ServerCallContext context)
        {
            return "bff3b2dd-e89d-46fc-a868-aab93a3efbbe";
        }
        public override async Task<TokenListGm> GetAll(TokenQueryFilter request, ServerCallContext context)
        {
            TokenListGm response = new TokenListGm();

            //PaginationGm pagination = new PaginationGm();

            IQueryable<Token> query = _tokenRepository.GetQuery();


            response.CurrentPage = request.PageId;
            response.ItemCount = await _tokenRepository.GetCount(query);
            response.PageCount = (response.ItemCount / request.PageSize) + 1;


            var tokens = from prd in await _tokenRepository.GetAll(query, request.PageId, request.PageSize)
                         select new TokenGm()
                         {
                             Id = prd.TokenId.ToString(),
                             Description = prd.Description,
                             Link = prd.Link,
                             Symbol = prd.Symbol,
                             Icon = prd.Icon,
                             Logo = prd.Logo,
                             Name = prd.Name,
                             Priority = prd.Priority,
                         };




            response.Items.AddRange(tokens.ToArray());
            //response.Pagination = pagination;
            return await Task.FromResult(response);

        }
        public override async Task<TokenListGm> GetByPartnerId(PartnerTokenQueryFilter request, ServerCallContext context)
        {
            TokenListGm response = new TokenListGm();

            //PaginationGm pagination = new PaginationGm();

            IQueryable<Token> query = _tokenRepository.GetQuery();
            query = query.Where(x => x.PartnerTokens.Any(c => c.PartnerId == Guid.Parse(request.PartnerId)));

            response.CurrentPage = request.PageId;
            response.ItemCount = await _tokenRepository.GetCount(query);
            response.PageCount = (response.ItemCount / request.PageSize) + 1;


            var tokens = from prd in await _tokenRepository.GetAll(query, request.PageId, request.PageSize)
                             select new TokenGm()
            {
                Id = prd.TokenId.ToString(),
                Description=prd.Description,
                Link=prd.Link,
                Symbol=prd.Symbol,
                Icon = prd.Icon,
                Logo = prd.Logo,
                Name = prd.Name,
                Priority = prd.Priority,
            };




            response.Items.AddRange(tokens.ToArray());
            //response.Pagination = pagination;
            return await Task.FromResult(response);

        }
        [AllowAnonymous]
        public override async Task<TokenFrontListGm> GetFrontTokenList(Empty request, ServerCallContext context)
        {
            TokenFrontListGm response = new TokenFrontListGm();

            //PaginationGm pagination = new PaginationGm();

            IQueryable<Token> query = _tokenRepository.GetQuery();



            var coins = from coin in await _tokenRepository.GetAll()
                        select new TokenFrontGm()
                        {
                            Coin = coin.Name,
                            Img = coin.Logo,
                            TotalCoin = 100,
                        };
            response.Items.AddRange(coins.ToArray());
            //response.Pagination = pagination;
            return await Task.FromResult(response);

        }
        public override async Task<TokenGm> GetById(TokenIdFilter request,ServerCallContext context)
        {
            var Token =await _tokenService.Find(Guid.Parse(request.TokenId));
            var searchedToken = new TokenGm()
            {
               Id=Token.TokenId.ToString(),
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
                CreatedById = getUserId(context),
                CreatedDate = DateTime.Now,
                Published = true,
                Symbol=request.Symbol,
                Link=request.Link,
                Description=request.Description,
                
            };

            var res = await _tokenService.Add(prdAdded);

            var response = new TokenGm()
            {
                Id = res.TokenId.ToString(),
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


        public override async Task<Empty> SavePartnerToken(PartnerTokenGm request, ServerCallContext context)
        {
            var partner =await _partnerRepository.Find(request.PartnerId);

            if(partner==null)
            {
               await _partnerRepository.Add(new Partner { PartnerId = Guid.Parse(request.PartnerId) });
            }
            PartnerToken item = await _partnerTokenRepository.Find(Guid.Parse(request.TokenId), Guid.Parse(request.PartnerId));

            if (item == null)
            {
                var partnerToken = new PartnerToken()
                {
                    CreatedById = getUserId(context),

                    CreatedDate = DateTime.Now,
                    Deleted = false,
                    PartnerId = Guid.Parse(request.PartnerId),
                    TokenId = Guid.Parse(request.TokenId),
                    Published = true,
                };

                await _partnerTokenRepository.Add(partnerToken);
            }
            else
            {
                await _partnerTokenRepository.Delete(item);
            }

            return await Task.FromResult<Empty>(new Empty());
        }

        public override async Task<Empty> DeletePartnerToken(PartnerTokenGm request, ServerCallContext context)
        {
            PartnerToken partnerToken = await _partnerTokenRepository.Find(Guid.Parse(request.TokenId), Guid.Parse(request.PartnerId));
            if (partnerToken == null)
            {
                return await Task.FromResult<Empty>(null);
            }
            await _partnerTokenRepository.Delete(partnerToken);
            return await Task.FromResult<Empty>(new Empty());
        }

        public override async Task<TokenGm> Put(TokenGm request,
           ServerCallContext context)
        {
            Token token = await _tokenService.Find(Guid.Parse(request.Id));
            if (token == null)
            {
                return await Task.FromResult<TokenGm>(null);
            }


            token.Name = request.Name;
            token.Logo = request.Logo;
            token.Icon = request.Icon;
            token.Priority = request.Priority;
            token.Description = request.Description;
            token.UpdatedById = getUserId(context);
            token.UpdatedDate = DateTime.Now;
            token.Symbol = request.Symbol;
            token.Icon = request.Icon;
            token.Link = request.Link;





            await _tokenService.Update(token);
            return await Task.FromResult<TokenGm>(new TokenGm()
            {
                Id = token.TokenId.ToString(),
                Icon = token.Icon,
                Logo = token.Logo,
                Name = token.Name,
                Priority = token.Priority,
                Link= token.Link,
                Symbol= token.Symbol,
                Description= token.Description,
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

        public override async Task<Empty> SoftDelete(TokenIdFilter request, ServerCallContext context)
        {
            Token token = await _tokenService.Find(Guid.Parse(request.TokenId));

            if (token == null)
            {
                return await Task.FromResult<Empty>(null);
            }

            token.Deleted = true;
            token.UpdatedById = getUserId(context);
            token.UpdatedDate = DateTime.Now;

            await _tokenRepository.Update(token);
            return await Task.FromResult<Empty>(new Empty());
        }
        public override async Task<Empty> SoftDeleteCollection(DeleteCollectionGm request, ServerCallContext context)
        {

            foreach (var item in request.Items)
            {
                Token token = await _tokenService.Find(Guid.Parse(item.TokenId));

                if (token == null)
                {
                    return await Task.FromResult<Empty>(null);
                }

                token.Deleted = true;
                token.UpdatedById = getUserId(context);
                token.UpdatedDate = DateTime.Now;

                await _tokenRepository.Update(token);
            }
            
            return await Task.FromResult<Empty>(new Empty());
        }
    }
}
