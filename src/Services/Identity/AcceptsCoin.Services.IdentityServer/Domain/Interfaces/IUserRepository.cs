using AcceptsCoin.Services.IdentityServer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcceptsCoin.Services.IdentityServer.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();
        Task<User> Find(Guid Id);

        Task<User> Find(string userName);
        Task<User> Add(User entity);

        Task<User> Update(User entity);

        Task Delete(User entity);
    }
}
