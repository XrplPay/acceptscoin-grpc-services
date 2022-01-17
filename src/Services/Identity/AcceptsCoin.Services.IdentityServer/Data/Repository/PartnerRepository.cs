using System;
using System.Threading.Tasks;
using AcceptsCoin.Services.IdentityServer.Data.Context;
using AcceptsCoin.Services.IdentityServer.Domain.Interfaces;
using AcceptsCoin.Services.IdentityServer.Domain.Models;

namespace AcceptsCoin.Services.IdentityServer.Data.Repository
{
    

    public class PartnerRepository : IPartnerRepository
    {

        private AcceptsCoinIdentityDbContext _context;

        public PartnerRepository(AcceptsCoinIdentityDbContext context)
        {
            _context = context;
        }



        public async Task<Partner> Add(Partner entity)
        {
            await _context.Partners.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Partner> Find(string Id)
        {
            return await _context.Partners.FindAsync(Guid.Parse(Id));
        }


    }
}
