using System;
using System.Threading.Tasks;
using AcceptsCoin.Services.DirectoryServer.Domain.Models;

namespace AcceptsCoin.Services.DirectoryServer.Domain.Interfaces
{
    public interface ICategoryRepository
    {

        Task<Category> Find(string Id);

        Task<Category> Add(Category entity);

    }
}
