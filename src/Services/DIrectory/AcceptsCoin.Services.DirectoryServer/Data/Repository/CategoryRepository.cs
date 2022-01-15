using System;
using System.Threading.Tasks;
using AcceptsCoin.Services.DirectoryServer.Data.Context;
using AcceptsCoin.Services.DirectoryServer.Domain.Interfaces;
using AcceptsCoin.Services.DirectoryServer.Domain.Models;

namespace AcceptsCoin.Services.DirectoryServer.Data.Repository
{
    public class CategoryRepository : ICategoryRepository
    {

        private AcceptsCoinDirectoryDbContext _context;

        public CategoryRepository(AcceptsCoinDirectoryDbContext context)
        {
            _context = context;
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

        
    }
}
