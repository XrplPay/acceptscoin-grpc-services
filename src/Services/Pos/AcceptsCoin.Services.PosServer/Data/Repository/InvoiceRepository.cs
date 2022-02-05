using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.Services.PosServer.Data.Context;
using AcceptsCoin.Services.PosServer.Domain.Interfaces;
using AcceptsCoin.Services.PosServer.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AcceptsCoin.Services.PosServer.Data.Repository
{

    public class InvoiceRepository : IInvoiceRepository
    {

        private AcceptsCoinPosDbContext _context;

        public InvoiceRepository(AcceptsCoinPosDbContext context)
        {
            _context = context;
        }

        public IQueryable<Invoice> GetQuery()
        {
            return _context.Invoices;
        }

        public async Task<int> GetCount(IQueryable<Invoice> query)
        {
            return await query.CountAsync();
        }

        public async Task<Invoice> Add(Invoice entity)
        {
            await _context.Invoices.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Invoice> Find(string Id)
        {
            return await _context.Invoices
                .Where(x => x.InvoiceId == Guid.Parse(Id)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Invoice>> GetAll()
        {

            
            return await _context.Invoices.ToListAsync();

        }
        public async Task<IEnumerable<Invoice>> GetAll(IQueryable<Invoice> query, int pageId, int pageSize)
        {

            var skip = (pageId - 1) * pageSize;
            var take = pageSize;

            return await query
                .Where(x => x.Deleted == false).Skip(skip).Take(take).ToListAsync();

        }

        public async Task<Invoice> Update(Invoice entity)
        {
            _context.Invoices.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(Invoice entity)
        {
            _context.Invoices.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
