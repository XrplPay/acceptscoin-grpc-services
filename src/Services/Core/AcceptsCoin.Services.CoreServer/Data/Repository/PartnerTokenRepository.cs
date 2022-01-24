using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.Services.CoreServer.Data.Context;
using AcceptsCoin.Services.CoreServer.Domain.Interfaces;
using AcceptsCoin.Services.CoreServer.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AcceptsCoin.Services.CoreServer.Data.Repository
{
    public class PartnerTokenRepository : IPartnerTokenRepository
    {

        private AcceptsCoinCoreDbContext _context;

        public PartnerTokenRepository(AcceptsCoinCoreDbContext context)
        {
            _context = context;
        }

        public IQueryable<PartnerToken> GetQuery()
        {
            return _context.PartnerTokens;
        }

        public async Task<int> GetCount(IQueryable<PartnerToken> query)
        {
            return await query.CountAsync();
        }

        public async Task<IEnumerable<PartnerToken>> GetAll(IQueryable<PartnerToken> query, int pageId, int pageSize)
        {

            var skip = (pageId - 1) * pageSize;
            var take = pageSize;

            return await query.Where(x => x.Deleted == false).Skip(skip).Take(take).ToListAsync();

        }

        public async Task<PartnerToken> Add(PartnerToken entity)
        {
            await _context.PartnerTokens.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        
       

        public async Task<IEnumerable<PartnerToken>> GetAll()
        {
            return await _context.PartnerTokens.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<PartnerToken> Update(PartnerToken entity)
        {
            _context.PartnerTokens.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(PartnerToken entity)
        {
            _context.PartnerTokens.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<PartnerToken> Find(Guid TokenId, Guid PartnerId)
        {
            return await _context.PartnerTokens.Where(x => x.PartnerId == PartnerId && x.TokenId == TokenId).SingleOrDefaultAsync();
        }

        
    }
}
