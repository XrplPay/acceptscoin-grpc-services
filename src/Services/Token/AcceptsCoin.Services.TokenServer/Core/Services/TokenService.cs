using AcceptsCoin.Services.TokenServer.Core.Interfaces;
using AcceptsCoin.Services.TokenServer.Domain.Interfaces;
using AcceptsCoin.Services.TokenServer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcceptsCoin.Services.TokenServer.Core.Services
{
    public class TokenService : ITokenService
    {

        private readonly ITokenRepository _tokenRepository;
        public TokenService(ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        public async Task<Token> Add(Token entity)
        {
            return await _tokenRepository.Add(entity);
        }

        public async Task Delete(Token entity)
        {
            await _tokenRepository.Delete(entity);
        }

        public async Task<Token> Find(Guid Id)
        {
            return await _tokenRepository.Find(Id);
        }

        public async Task<IEnumerable<Token>> GetAll()
        {
            return await _tokenRepository.GetAll();
        }

        public async Task<Token> Update(Token entity)
        {
            return await _tokenRepository.Update(entity);
        }
    }
}
