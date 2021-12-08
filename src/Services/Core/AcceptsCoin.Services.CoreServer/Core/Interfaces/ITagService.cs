using AcceptsCoin.Services.CoreServer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcceptsCoin.Services.CoreServer.Core.Interfaces
{
    public interface ITagService
    {
        Task<IEnumerable<Tag>> GetAll();
        Task<Tag> Find(Guid Id);

        Task<Tag> Add(Tag entity);

        Task<Tag> Update(Tag entity);

        Task Delete(Tag entity);

        Task SoftDelete(Tag entity);
    }
}
