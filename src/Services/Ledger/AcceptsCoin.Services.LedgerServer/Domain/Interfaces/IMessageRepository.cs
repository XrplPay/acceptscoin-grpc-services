using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.Services.LedgerServer.Domain.Models;

namespace AcceptsCoin.Services.LedgerServer.Domain.Interfaces
{
    public interface IMessageRepository
    {
        IQueryable<Message> GetQuery();

        Task<int> GetCount(IQueryable<Message> query);

        Task<IEnumerable<Message>> GetAll();

        Task<IEnumerable<Message>> GetAll(IQueryable<Message> query, int pageId, int pageSize);

        Task<Message> Find(string Id);

        Task<Message> Add(Message entity);

        Task<Message> Update(Message entity);

        Task Delete(Message entity);
    }
}
