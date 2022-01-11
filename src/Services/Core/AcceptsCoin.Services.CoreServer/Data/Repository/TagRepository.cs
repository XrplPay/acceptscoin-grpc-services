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
    public class TagRepository : ITagRepository
    {

        private AcceptsCoinCoreDbContext _context;

        public TagRepository(AcceptsCoinCoreDbContext context)
        {
            _context = context;
        }


        public IQueryable<Tag> GetQuery()
        {
            return _context.Tags;
        }

        public async Task<int> GetCount(IQueryable<Tag> query)
        {
            return await query.CountAsync();
        }

        public async Task<IEnumerable<Tag>> GetAll(IQueryable<Tag> query, int pageId, int pageSize)
        {

            var skip = (pageId - 1) * pageSize;
            var take = pageSize;

            return await query.Where(x => x.Deleted == false).Skip(skip).Take(take).ToListAsync();

        }
        public async Task<Tag> Add(Tag entity)
        {
            await _context.Tags.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Tag> Find(Guid Id)
        {
            return await _context.Tags.FindAsync(Id);
        }
       

        public async Task<IEnumerable<Tag>> GetAll()
        {
            return await _context.Tags.ToListAsync();
        }

        public async Task<Tag> Update(Tag entity)
        {
            _context.Tags.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(Tag entity)
        {
            _context.Tags.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
