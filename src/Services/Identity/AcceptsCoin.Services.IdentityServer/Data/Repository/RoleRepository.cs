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
    public class RoleRepository : IRoleRepository
    {

        private AcceptsCoinIdentityDbContext _context;

        public RoleRepository(AcceptsCoinIdentityDbContext context)
        {
            _context = context;
        }

        public IQueryable<Role> GetQuery()
        {
            return _context.Roles;
        }

        public async Task<int> GetCount(IQueryable<Role> query)
        {
            return await query.CountAsync();
        }

       

        public async Task<Role> Add(Role entity)
        {
            await _context.Roles.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Role> Find(Guid Id)
        {
            return await _context.Roles.FindAsync(Id);
        }
        public async Task<Role> Find(string Name)
        {
            return await _context.Roles.Where(x => x.Name == Name).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Role>> GetAll(IQueryable<Role> query, int pageId, int pageSize)
        {
            var skip = (pageId - 1) * pageSize;
            var take = pageSize;

            return await query.Where(x => x.Deleted == false).Skip(skip).Take(take).ToListAsync();
        }

        public async Task<Role> Update(Role entity)
        {
            _context.Roles.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(Role entity)
        {
            _context.Roles.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
