using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.Services.SmsSenderServer.Data.Context;
using AcceptsCoin.Services.SmsSenderServer.Domain.Interfaces;
using AcceptsCoin.Services.SmsSenderServer.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AcceptsCoin.Services.SmsSenderServer.Data.Repository
{

    public class MessageRepository : IMessageRepository
    {

        private AcceptsCoinSmsSenderDbContext _context;

        public MessageRepository(AcceptsCoinSmsSenderDbContext context)
        {
            _context = context;
        }

        public IQueryable<Message> GetQuery()
        {
            return _context.Messages;
        }

        public async Task<int> GetCount(IQueryable<Message> query)
        {
            return await query.CountAsync();
        }

        public async Task<Message> Add(Message entity)
        {
            await _context.Messages.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Message> Find(string Id)
        {
            return await _context.Messages
                .Where(x => x.MessageId == Guid.Parse(Id)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Message>> GetAll()
        {
            return await _context.Messages.ToListAsync();
        }
        public async Task<IEnumerable<Message>> GetAll(IQueryable<Message> query, int pageId, int pageSize)
        {

            var skip = (pageId - 1) * pageSize;
            var take = pageSize;

            return await query
                .Where(x => x.Deleted == false).Skip(skip).Take(take).ToListAsync();

        }

        public async Task<Message> Update(Message entity)
        {
            _context.Messages.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(Message entity)
        {
            _context.Messages.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
