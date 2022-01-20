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
    public class BusinessTagRepository : IBusinessTagRepository
    {

        private AcceptsCoinDirectoryDbContext _context;

        public BusinessTagRepository(AcceptsCoinDirectoryDbContext context)
        {
            _context = context;
        }

        public IQueryable<BusinessTag> GetQuery()
        {
            return _context.BusinessTags;
        }

        public async Task<int> GetCount(IQueryable<BusinessTag> query)
        {
            return await query.CountAsync();
        }

        public async Task<IEnumerable<BusinessTag>> GetAll(IQueryable<BusinessTag> query, int pageId, int pageSize)
        {

            var skip = (pageId - 1) * pageSize;
            var take = pageSize;

            return await query.Skip(skip).Take(take).ToListAsync();

        }

        public async Task<BusinessTag> Add(BusinessTag entity)
        {
            await _context.BusinessTags.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        
       

        public async Task<IEnumerable<BusinessTag>> GetAll()
        {
            return await _context.BusinessTags.ToListAsync();
        }

        public async Task<BusinessTag> Update(BusinessTag entity)
        {
            _context.BusinessTags.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(BusinessTag entity)
        {
            _context.BusinessTags.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<BusinessTag> Find(Guid BusinessId, Guid TagId)
        {
            return await _context.BusinessTags.Where(x => x.BusinessId == BusinessId && x.TagId == TagId).SingleOrDefaultAsync();
        }

        
    }
}
