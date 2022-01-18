using AcceptsCoin.Services.IdentityServer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcceptsCoin.Services.IdentityServer.Domain.Interfaces
{
    public interface IRoleRepository
    {
        IQueryable<Role> GetQuery();

        Task<int> GetCount(IQueryable<Role> query);

        Task<IEnumerable<Role>> GetAll(IQueryable<Role> query, int pageId, int pageSize);

        Task<Role> Find(Guid Id);

        Task<Role> Find(string RoleName);

        Task<Role> Add(Role entity);

        Task<Role> Update(Role entity);

        Task Delete(Role entity);
    }
}
