using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AcceptsCoin.Services.DirectoryServer.Domain.Models;

namespace AcceptsCoin.Services.DirectoryServer.Domain.Interfaces
{
    public interface IBusinessRepository
    {
        Task<IEnumerable<Business>> GetAll();
        Task<Business> Find(string Id);
        Task<Business> Add(Business entity);
        Task<Business> Update(Business entity);
        Task Delete(Business entity);
    }
}
