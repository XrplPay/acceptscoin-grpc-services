using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AcceptsCoin.Services.DirectoryServer.Core.Interfaces;
using AcceptsCoin.Services.DirectoryServer.Domain.Interfaces;
using AcceptsCoin.Services.DirectoryServer.Domain.Models;

namespace AcceptsCoin.Services.DirectoryServer.Core.Services
{
    public class BusinessService : IBusinessService
    {

        private readonly IBusinessRepository _businessRepository;
        public BusinessService(IBusinessRepository businessRepository)
        {
            _businessRepository = businessRepository;
        }

        public async Task<IEnumerable<Business>> GetAll()
        {

            return await _businessRepository.GetAll();
        }

        public async Task<Business> Add(Business entity)
        {
            return await _businessRepository.Add(entity);
        }

        public async Task Delete(Business entity)
        {
            await _businessRepository.Delete(entity);
        }

        public async Task<Business> Find(string Id)
        {
            return await _businessRepository.Find(Id);
        }

        

        public async Task SoftDelete(Business entity)
        {
            entity.Deleted = true;
            await _businessRepository.Update(entity);
        }

        public async Task<Business> Update(Business entity)
        {
            return await _businessRepository.Update(entity);
        }
    }
}
