using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.Services.DirectoryServer.Domain;
using AcceptsCoin.Services.DirectoryServer.Domain.Models;

namespace AcceptsCoin.Services.DirectoryServer.Domain.Interfaces
{
    public interface IReviewRepository
    {
        IQueryable<Review> GetQuery();

        Task<int> GetCount(IQueryable<Review> query);

        Task<IEnumerable<Review>> GetAll(IQueryable<Review> query, int pageId, int pageSize);

        Task<IEnumerable<Review>> GetAll();

        Task<Review> Find(Guid Id);

        Task<Review> Add(Review entity);

        Task<Review> Update(Review entity);

        Task Delete(Review entity);
    }
}
