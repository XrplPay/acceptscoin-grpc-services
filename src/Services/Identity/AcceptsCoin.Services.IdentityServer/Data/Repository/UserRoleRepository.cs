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
    public class UserRoleRepository : IUserRoleRepository
    {

        private AcceptsCoinIdentityDbContext _context;

        public UserRoleRepository(AcceptsCoinIdentityDbContext context)
        {
            _context = context;
        }

        public IQueryable<UserRole> GetQuery()
        {
            return _context.UserRoles;
        }

        public async Task<int> GetCount(IQueryable<UserRole> query)
        {
            return await query.CountAsync();
        }

        public async Task<IEnumerable<UserRole>> GetAll(IQueryable<UserRole> query, int pageId, int pageSize)
        {

            var skip = (pageId - 1) * pageSize;
            var take = pageSize;

            return await query.Skip(skip).Take(take).ToListAsync();

        }

        public async Task<UserRole> Add(UserRole entity)
        {
            await _context.UserRoles.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        
       

        public async Task<IEnumerable<UserRole>> GetAll()
        {
            return await _context.UserRoles.ToListAsync();
        }

        public async Task<UserRole> Update(UserRole entity)
        {
            _context.UserRoles.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(UserRole entity)
        {
            _context.UserRoles.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<UserRole> Find(Guid UserId, Guid RoleId)
        {
            return await _context.UserRoles.Where(x => x.UserId == UserId && x.RoleId == RoleId).SingleOrDefaultAsync();
        }

        
    }
}
