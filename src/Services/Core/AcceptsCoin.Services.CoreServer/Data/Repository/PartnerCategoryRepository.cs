using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.Services.CoreServer.Data.Context;
using AcceptsCoin.Services.CoreServer.Domain.Interfaces;
using AcceptsCoin.Services.CoreServer.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AcceptsCoin.Services.TokenServer.Data.Repository
{
    public class PartnerCategoryRepository : IPartnerCategoryRepository
    {

        private AcceptsCoinCoreDbContext _context;

        public PartnerCategoryRepository(AcceptsCoinCoreDbContext context)
        {
            _context = context;
        }

        public IQueryable<PartnerCategory> GetQuery()
        {
            return _context.PartnerCategories;
        }

        public async Task<int> GetCount(IQueryable<PartnerCategory> query)
        {
            return await query.CountAsync();
        }

        public async Task<IEnumerable<PartnerCategory>> GetAll(IQueryable<PartnerCategory> query, int pageId, int pageSize)
        {

            var skip = (pageId - 1) * pageSize;
            var take = pageSize;

            return await query.Where(x => x.Deleted == false).Skip(skip).Take(take).ToListAsync();

        }

        public async Task<PartnerCategory> Add(PartnerCategory entity)
        {
            await _context.PartnerCategories.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        
       

        public async Task<IEnumerable<PartnerCategory>> GetAll()
        {
            return await _context.PartnerCategories.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<PartnerCategory> Update(PartnerCategory entity)
        {
            _context.PartnerCategories.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(PartnerCategory entity)
        {
            _context.PartnerCategories.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<PartnerCategory> Find(Guid CategoryId, Guid PartnerId)
        {
            return await _context.PartnerCategories.Where(x => x.PartnerId == PartnerId && x.CategoryId == CategoryId).SingleOrDefaultAsync();
        }

        
    }
}
