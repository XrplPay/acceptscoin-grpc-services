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
    public class LanguageRepository : ILanguageRepository
    {

        private AcceptsCoinCoreDbContext _context;

        public LanguageRepository(AcceptsCoinCoreDbContext context)
        {
            _context = context;
        }

        public async Task<Language> Add(Language entity)
        {
            await _context.Languages.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Language> Find(Guid Id)
        {
            return await _context.Languages.FindAsync(Id);
        }
       

        public async Task<IEnumerable<Language>> GetAll()
        {
            return await _context.Languages.ToListAsync();
        }

        public async Task<Language> Update(Language entity)
        {
            _context.Languages.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(Language entity)
        {
            _context.Languages.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
