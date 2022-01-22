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
    public class ReviewRepository : IReviewRepository
    {

        private AcceptsCoinDirectoryDbContext _context;

        public ReviewRepository(AcceptsCoinDirectoryDbContext context)
        {
            _context = context;
        }


        public IQueryable<Review> GetQuery()
        {
            return _context.Reviews;
        }

        public async Task<int> GetCount(IQueryable<Review> query)
        {
            return await query.CountAsync();
        }

        public async Task<IEnumerable<Review>> GetAll(IQueryable<Review> query, int pageId, int pageSize)
        {

            var skip = (pageId - 1) * pageSize;
            var take = pageSize;

            return await query.Where(x => x.Deleted == false).Include(x => x.CreatedBy).Skip(skip).Take(take).ToListAsync();

        }
        public async Task<Review> Add(Review entity)
        {
            await _context.Reviews.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Review> Find(Guid Id)
        {
            return await _context.Reviews.Where(x => x.ReviewId == Id).FirstOrDefaultAsync();
        }
       

        public async Task<IEnumerable<Review>> GetAll()
        {
            return await _context.Reviews.ToListAsync();
        }

        public async Task<Review> Update(Review entity)
        {
            _context.Reviews.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(Review entity)
        {
            _context.Reviews.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
