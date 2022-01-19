using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.Services.IdentityServer.Data.Context;
using AcceptsCoin.Services.IdentityServer.Domain.Interfaces;
using AcceptsCoin.Services.IdentityServer.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AcceptsCoin.Services.IdentityServer.Data.Repository
{
    public class UserRepository : IUserRepository
    {

        private AcceptsCoinIdentityDbContext _context;

        public UserRepository(AcceptsCoinIdentityDbContext context)
        {
            _context = context;
        }

        public IQueryable<User> GetQuery()
        {
            return _context.Users;
        }

        public async Task<int> GetCount(IQueryable<User> query)
        {
            return await query.CountAsync();
        }

       

        public async Task<User> Add(User entity)
        {
            await _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<User> Find(Guid Id)
        {
            return await _context.Users.Where(x => x.UserId == Id).FirstOrDefaultAsync();
        }
        public async Task<User> Find(string userName)
        {
            return await _context.Users.Where(x => x.UserName == userName).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> GetAll(IQueryable<User> query, int pageId, int pageSize)
        {
            var skip = (pageId - 1) * pageSize;
            var take = pageSize;

            return await query.Where(x => x.Deleted == false).Skip(skip).Take(take).ToListAsync();
        }

        public async Task<User> Update(User entity)
        {
            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(User entity)
        {
            _context.Users.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
