using AcceptsCoin.Services.DirectoryServer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcceptsCoin.Services.DirectoryServer.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> Find(Guid Id);

        Task<User> Add(User entity);

        Task<User> Update(User entity);
    }
}
