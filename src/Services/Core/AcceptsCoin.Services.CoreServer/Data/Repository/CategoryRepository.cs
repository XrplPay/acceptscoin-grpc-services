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
    public class CategoryRepository : ICategoryRepository
    {

        private AcceptsCoinCoreDbContext _context;

        public CategoryRepository(AcceptsCoinCoreDbContext context)
        {
            _context = context;
        }

        public IQueryable<Category> GetQuery()
        {
            return _context.Categories;
        }

        public async Task<int> GetCount(IQueryable<Category> query)
        {
            return await query.CountAsync();
        }

        public async Task<IEnumerable<Category>> GetAll(IQueryable<Category> query, int pageId, int pageSize)
        {

            var skip = (pageId - 1) * pageSize;
            var take = pageSize;

            return await query.Where(x => x.Deleted == false).Skip(skip).Take(take).ToListAsync();

        }

        public async Task<IEnumerable<Category>> GetAll(Guid? parentId)
        {
            return await _context.Categories.Where(x => x.Deleted == false && x.ParentId == parentId).ToListAsync();
        }

     


        public async Task<Category> Add(Category entity)
        {
            await _context.Categories.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Category> Find(string Id)
        {
            return await _context.Categories.FindAsync(Guid.Parse(Id));
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> Update(Category entity)
        {
            _context.Categories.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(Category entity)
        {
            _context.Categories.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
