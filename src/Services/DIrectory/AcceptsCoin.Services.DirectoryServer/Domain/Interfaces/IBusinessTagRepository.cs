using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AcceptsCoin.Services.DirectoryServer.Domain.Models;

namespace AcceptsCoin.Services.DirectoryServer.Domain.Interfaces
{
    public interface IBusinessTagRepository
    {
        Task<IEnumerable<BusinessTag>> GetAll();
        Task<BusinessTag> Find(string Id);
        Task<BusinessTag> Add(BusinessTag entity);
        Task<BusinessTag> Update(BusinessTag entity);
        Task Delete(BusinessTag entity);
    }
}
