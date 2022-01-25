using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.Services.DirectoryServer.Data.Context;
using AcceptsCoin.Services.DirectoryServer.Domain.Interfaces;
using AcceptsCoin.Services.DirectoryServer.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AcceptsCoin.Services.DirectoryServer.Data.Repository
{
    

    public class TagRepository : ITagRepository
    {

        private AcceptsCoinDirectoryDbContext _context;

        public TagRepository(AcceptsCoinDirectoryDbContext context)
        {
            _context = context;
        }



        public async Task<Tag> Add(Tag entity)
        {
            await _context.Tags.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Tag> Find(string Id)
        {
            return await _context.Tags.Where(x=>x.TagId==(Guid.Parse(Id))).FirstOrDefaultAsync();
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

            return await query.Skip(skip).Take(take).ToListAsync();

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
