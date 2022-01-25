using System;
using System.Linq;
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
        private ITagRepository _tagRepository;
        public DirectoryGrpcService(ILogger<DirectoryGrpcService> logger, IUserRepository userRepository, ITokenRepository tokenRepository, ITagRepository tagRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _tagRepository = tagRepository;
        }

        [AllowAnonymous]
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
                token = new Token()
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
            else
            {
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


        public override async Task<DirectoryTokenListGm> GetTokenListByBusinessId(TokenBusinessIdQueryFilter request, ServerCallContext context)
        {
            DirectoryTokenListGm response = new DirectoryTokenListGm();

            IQueryable<Token> query = _tokenRepository.GetQuery();
            query = query.Where(x => x.BusinessTokens.Any(x => x.BusinessId == Guid.Parse(request.BusinessId)));
            
            response.CurrentPage = request.PageId;
            response.ItemCount = await _tokenRepository.GetCount(query);
            response.PageCount = (response.ItemCount / request.PageSize) + 1;


            var tokens = from token in await _tokenRepository.GetAll(query, request.PageId, request.PageSize)
                         select new DirectoryTokenGm()
                         {

                             Icon = token.Icon,
                             Id = token.TokenId.ToString(),
                             Logo = token.Logo,
                             Name = token.Name,
                             Symbol = token.Symbol,
                         };
            response.Items.AddRange(tokens.ToArray());
            return await Task.FromResult(response);
        }

        public override async Task<DirectoryTagListGm> GetTagListByBusinessId(TagBusinessIdQueryFilter request, ServerCallContext context)
        {
            DirectoryTagListGm response = new DirectoryTagListGm();

            IQueryable<Tag> query = _tagRepository.GetQuery();
            query = query.Where(x => x.BusinessTags.Any(x => x.BusinessId == Guid.Parse(request.BusinessId)));

            response.CurrentPage = request.PageId;
            response.ItemCount = await _tagRepository.GetCount(query);
            response.PageCount = (response.ItemCount / request.PageSize) + 1;


            var tags = from tag in await _tagRepository.GetAll(query, request.PageId, request.PageSize)
                       select new DirectoryTagGm()
                       {
                           Id = tag.TagId.ToString(),
                           Title = "TAG TITLE",
                       };
            response.Items.AddRange(tags.ToArray());
            return await Task.FromResult(response);
        }


    }

}