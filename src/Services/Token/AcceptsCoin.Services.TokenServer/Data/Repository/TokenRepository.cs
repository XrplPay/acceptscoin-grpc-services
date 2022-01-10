using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.Services.TokenServer.Data.Context;
using AcceptsCoin.Services.TokenServer.Domain.Interfaces;
using AcceptsCoin.Services.TokenServer.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AcceptsCoin.Services.TokenServer.Data.Repository
{
    public class TokenRepository : ITokenRepository
    {

        private AcceptsCoinTokenDbContext _context;

        public TokenRepository(AcceptsCoinTokenDbContext context)
        {
            _context = context;
        }

        public IQueryable<Token> GetQuery()
        {
            return _context.Tokens;
        }

        public async Task<int> GetCount(IQueryable<Token> query)
        {
            return await query.CountAsync();
        }

        public async Task<IEnumerable<Token>> GetAll(IQueryable<Token> query, int pageId, int pageSize)
        {

            var skip = (pageId - 1) * pageSize;
            var take = pageSize;

            return await query.Where(x => x.Deleted == false).Skip(skip).Take(take).ToListAsync();

        }

        public async Task<Token> Add(Token entity)
        {
            await _context.Tokens.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Token> Find(Guid Id)
        {
            return await _context.Tokens.FindAsync(Id);
        }
       

        public async Task<IEnumerable<Token>> GetAll()
        {
            return await _context.Tokens.ToListAsync();
        }

        public async Task<Token> Update(Token entity)
        {
            _context.Tokens.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(Token entity)
        {
            _context.Tokens.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
