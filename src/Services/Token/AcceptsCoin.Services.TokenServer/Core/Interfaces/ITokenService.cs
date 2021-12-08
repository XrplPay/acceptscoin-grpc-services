using AcceptsCoin.Services.TokenServer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcceptsCoin.Services.TokenServer.Core.Interfaces
{
    public interface ITokenService
    {
        Task<IEnumerable<Token>> GetAll();
        Task<Token> Find(Guid Id);

        Task<Token> Add(Token entity);

        Task<Token> Update(Token entity);

        Task Delete(Token entity);

        Task SoftDelete(Token entity);
    }
}
