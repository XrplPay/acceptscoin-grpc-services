using AcceptsCoin.Services.IdentityServer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcceptsCoin.Services.IdentityServer.Domain.Interfaces
{
    public interface IUserRoleRepository
    {
        IQueryable<UserRole> GetQuery();

        Task<int> GetCount(IQueryable<UserRole> query);

        Task<IEnumerable<UserRole>> GetAll(IQueryable<UserRole> query, int pageId, int pageSize);

        Task<IEnumerable<UserRole>> GetAll();

        Task<UserRole> Find(Guid UserId, Guid RoleId);

        Task<UserRole> Add(UserRole entity);

        Task<UserRole> Update(UserRole entity);

        Task Delete(UserRole entity);
    }
}
