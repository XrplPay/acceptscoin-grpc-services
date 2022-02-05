using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.Services.PosServer.Domain.Models;

namespace AcceptsCoin.Services.PosServer.Domain.Interfaces
{
    public interface IInvoiceRepository
    {
        IQueryable<Invoice> GetQuery();

        Task<int> GetCount(IQueryable<Invoice> query);

        Task<IEnumerable<Invoice>> GetAll();

        Task<IEnumerable<Invoice>> GetAll(IQueryable<Invoice> query, int pageId, int pageSize);

        Task<Invoice> Find(string Id);

        Task<Invoice> Add(Invoice entity);

        Task<Invoice> Update(Invoice entity);

        Task Delete(Invoice entity);
    }
}
