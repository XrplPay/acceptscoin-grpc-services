using System;
using System.Threading.Tasks;
using AcceptsCoin.Services.TokenServer.Data.Context;
using AcceptsCoin.Services.TokenServer.Domain.Interfaces;
using AcceptsCoin.Services.TokenServer.Domain.Models;

namespace AcceptsCoin.Services.TokenServer.Data.Repository
{
    

    public class PartnerRepository : IPartnerRepository
    {

        private AcceptsCoinTokenDbContext _context;

        public PartnerRepository(AcceptsCoinTokenDbContext context)
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
