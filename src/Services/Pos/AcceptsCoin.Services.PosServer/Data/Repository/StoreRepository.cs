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

    public class StoreRepository : IStoreRepository
    {

        private AcceptsCoinPosDbContext _context;

        public StoreRepository(AcceptsCoinPosDbContext context)
        {
            _context = context;
        }

        public IQueryable<Store> GetQuery()
        {
            return _context.Stores;
        }

        public async Task<int> GetCount(IQueryable<Store> query)
        {
            return await query.CountAsync();
        }

        public async Task<Store> Add(Store entity)
        {
            await _context.Stores.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Store> Find(string Id)
        {
            return await _context.Stores
                .Where(x => x.StoreId == Guid.Parse(Id)).FirstOrDefaultAsync();
        }

        public async Task<Store> FindByEmail(string email)
        {
            return await _context.Stores
                .Where(x => x.Email.ToLower() == email.ToLower()).FirstOrDefaultAsync();
        }


        public async Task<Store> FindByWebSiteUrl(string webSiteUrl)
        {
            return await _context.Stores
                .Where(x => x.WebSite.ToLower() == webSiteUrl.ToLower()).FirstOrDefaultAsync();
        }

        public async Task<Store> Find(string email, string webSiteUrl)
        {
            return await _context.Stores
                .Where(x => x.Email.ToLower() == email.ToLower() && x.WebSite.ToLower() == webSiteUrl.ToLower()).FirstOrDefaultAsync();
        }



        public async Task<IEnumerable<Store>> GetAll()
        {

            
            return await _context.Stores.ToListAsync();

        }
        public async Task<IEnumerable<Store>> GetAll(IQueryable<Store> query, int pageId, int pageSize)
        {

            var skip = (pageId - 1) * pageSize;
            var take = pageSize;

            return await query
                .Where(x => x.Deleted == false).Skip(skip).Take(take).ToListAsync();

        }

        public async Task<Store> Update(Store entity)
        {
            _context.Stores.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(Store entity)
        {
            _context.Stores.Remove(entity);
            await _context.SaveChangesAsync();
        }

        
    }
}
