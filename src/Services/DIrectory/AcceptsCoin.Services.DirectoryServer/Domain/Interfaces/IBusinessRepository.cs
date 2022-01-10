﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.Services.DirectoryServer.Domain.Models;

namespace AcceptsCoin.Services.DirectoryServer.Domain.Interfaces
{
    public interface IBusinessRepository
    {
        IQueryable<Business> GetQuery();

        Task<int> GetCount(IQueryable<Business> query);

        Task<IEnumerable<Business>> GetAll();

        Task<IEnumerable<Business>> GetAll(IQueryable<Business> query, int pageId, int pageSize);

        Task<Business> Find(string Id);

        Task<Business> Add(Business entity);

        Task<Business> Update(Business entity);

        Task Delete(Business entity);
    }
}
