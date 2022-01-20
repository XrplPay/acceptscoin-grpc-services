using System;
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


    }
}
