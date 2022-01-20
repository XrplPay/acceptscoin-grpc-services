using System;
using System.Threading.Tasks;
using AcceptsCoin.Services.DirectoryServer.Domain.Models;

namespace AcceptsCoin.Services.DirectoryServer.Domain.Interfaces
{
   
    public interface ITagRepository
    {

        Task<Tag> Find(string Id);

        Task<Tag> Add(Tag entity);

    }
}
