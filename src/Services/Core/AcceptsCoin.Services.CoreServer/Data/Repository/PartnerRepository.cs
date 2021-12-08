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
    public class PartnerRepository : IPartnerRepository
    {

        private AcceptsCoinCoreDbContext _context;

        public PartnerRepository(AcceptsCoinCoreDbContext context)
        {
            _context = context;
        }

        public async Task<Partner> Add(Partner entity)
        {
            await _context.Partners.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Partner> Find(Guid Id)
        {
            return await _context.Partners.FindAsync(Id);
        }
       

        public async Task<IEnumerable<Partner>> GetAll()
        {
            return await _context.Partners.ToListAsync();
        }

        public async Task<Partner> Update(Partner entity)
        {
            _context.Partners.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(Partner entity)
        {
            _context.Partners.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
