using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.Services.DirectoryServer.Domain.Models;

namespace AcceptsCoin.Services.DirectoryServer.Domain.Interfaces
{
    public interface IBusinessGalleryRepository
    {
        IQueryable<BusinessGallery> GetQuery();

        Task<int> GetCount(IQueryable<BusinessGallery> query);

        Task<IEnumerable<BusinessGallery>> GetAll();

        Task<IEnumerable<BusinessGallery>> GetAll(IQueryable<BusinessGallery> query, int pageId, int pageSize);

        Task<BusinessGallery> Find(string Id);

        Task<BusinessGallery> Add(BusinessGallery entity);

        Task<BusinessGallery> Update(BusinessGallery entity);

        Task Delete(BusinessGallery entity);
    }
}
