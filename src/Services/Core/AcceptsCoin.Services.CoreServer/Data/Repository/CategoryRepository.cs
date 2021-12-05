using System;
using System.Collections.Generic;
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
        public async Task<IEnumerable<Category>> GetAllCategory()
        {
            return await _context.Categories.ToListAsync();
        }
    }
}
