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

    public class BusinessGalleryRepository : IBusinessGalleryRepository
    {

        private AcceptsCoinDirectoryDbContext _context;

        public BusinessGalleryRepository(AcceptsCoinDirectoryDbContext context)
        {
            _context = context;
        }

        public IQueryable<BusinessGallery> GetQuery()
        {
            return _context.BusinessGalleries;
        }

        public async Task<int> GetCount(IQueryable<BusinessGallery> query)
        {
            return await query.CountAsync();
        }

        public async Task<BusinessGallery> Add(BusinessGallery entity)
        {
            await _context.BusinessGalleries.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<BusinessGallery> Find(string Id)
        {
            return await _context.BusinessGalleries.FindAsync(Guid.Parse(Id));
        }

        public async Task<IEnumerable<BusinessGallery>> GetAll()
        {

            
            return await _context.BusinessGalleries.ToListAsync();

        }
        public async Task<IEnumerable<BusinessGallery>> GetAll(IQueryable<BusinessGallery> query, int pageId, int pageSize)
        {

            var skip = (pageId - 1) * pageSize;
            var take = pageSize;

            return await query.Where(x => x.Deleted == false).Skip(skip).Take(take).ToListAsync();

        }

        public async Task<BusinessGallery> Update(BusinessGallery entity)
        {
            _context.BusinessGalleries.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(BusinessGallery entity)
        {
            _context.BusinessGalleries.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
