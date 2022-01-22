using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.Services.DirectoryServer.Data.Context;
using AcceptsCoin.Services.DirectoryServer.Domain.Interfaces;
using AcceptsCoin.Services.DirectoryServer.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AcceptsCoin.Services.DirectoryServer.Data.Repository
{
    public class BusinessTokenRepository : IBusinessTokenRepository
    {

        private AcceptsCoinDirectoryDbContext _context;

        public BusinessTokenRepository(AcceptsCoinDirectoryDbContext context)
        {
            _context = context;
        }

        public IQueryable<BusinessToken> GetQuery()
        {
            return _context.BusinessTokens;
        }

        public async Task<int> GetCount(IQueryable<BusinessToken> query)
        {
            return await query.CountAsync();
        }

        public async Task<IEnumerable<BusinessToken>> GetAll(IQueryable<BusinessToken> query, int pageId, int pageSize)
        {

            var skip = (pageId - 1) * pageSize;
            var take = pageSize;

            return await query.Skip(skip).Take(take).ToListAsync();

        }

        public async Task<BusinessToken> Add(BusinessToken entity)
        {
            await _context.BusinessTokens.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        
       

        public async Task<IEnumerable<BusinessToken>> GetAll()
        {
            return await _context.BusinessTokens.ToListAsync();
        }

        public async Task<BusinessToken> Update(BusinessToken entity)
        {
            _context.BusinessTokens.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(BusinessToken entity)
        {
            _context.BusinessTokens.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<BusinessToken> Find(Guid BusinessId, Guid TokenId)
        {
            return await _context.BusinessTokens.Where(x => x.BusinessId == BusinessId && x.TokenId == TokenId).SingleOrDefaultAsync();
        }

        
    }
}
