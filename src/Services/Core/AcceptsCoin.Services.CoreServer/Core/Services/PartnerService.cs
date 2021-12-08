using AcceptsCoin.Services.CoreServer.Core.Interfaces;
using AcceptsCoin.Services.CoreServer.Domain.Interfaces;
using AcceptsCoin.Services.CoreServer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcceptsCoin.Services.CoreServer.Core.Services
{
    public class PartnerService : IPartnerService
    {

        private readonly IPartnerRepository _partnerRepository;
        public PartnerService(IPartnerRepository partnerRepository)
        {
            _partnerRepository = partnerRepository;
        }

        public async Task<Partner> Add(Partner entity)
        {
            return await _partnerRepository.Add(entity);
        }

        public async Task Delete(Partner entity)
        {
            await _partnerRepository.Delete(entity);
        }

        public async Task<Partner> Find(Guid Id)
        {
            return await _partnerRepository.Find(Id);
        }

        public async Task<IEnumerable<Partner>> GetAll()
        {
            return await _partnerRepository.GetAll();
        }

        public async Task SoftDelete(Partner entity)
        {
            entity.Deleted = true;
            await _partnerRepository.Update(entity);
        }

        public async Task<Partner> Update(Partner entity)
        {
            return await _partnerRepository.Update(entity);
        }
    }
}
