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

    public class BusinessRepository : IBusinessRepository
    {

        private AcceptsCoinDirectoryDbContext _context;

        public BusinessRepository(AcceptsCoinDirectoryDbContext context)
        {
            _context = context;
        }

        public IQueryable<Business> GetQuery()
        {
            return _context.Businesses;
        }

        public async Task<int> GetCount(IQueryable<Business> query)
        {
            return await query.CountAsync();
        }

        public async Task<Business> Add(Business entity)
        {
            await _context.Businesses.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Business> Find(string Id)
        {
            return await _context.Businesses.Include(x => x.BusinessGalleries).Where(x => x.BusinessId == Guid.Parse(Id)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Business>> GetAll()
        {

            
            return await _context.Businesses.Include(x => x.BusinessGalleries).ToListAsync();

        }
        public async Task<IEnumerable<Business>> GetAll(IQueryable<Business> query, int pageId, int pageSize)
        {

            var skip = (pageId - 1) * pageSize;
            var take = pageSize;

            return await query.Include(x=>x.BusinessGalleries).Where(x => x.Deleted == false).Skip(skip).Take(take).ToListAsync();

        }

        public async Task<Business> Update(Business entity)
        {
            _context.Businesses.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(Business entity)
        {
            _context.Businesses.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
