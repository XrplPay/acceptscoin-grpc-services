using AcceptsCoin.Services.CoreServer.Core.Interfaces;
using AcceptsCoin.Services.CoreServer.Domain.Interfaces;
using AcceptsCoin.Services.CoreServer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcceptsCoin.Services.CoreServer.Core.Services
{
    public class TagService : ITagService
    {

        private readonly ITagRepository _tagRepository;
        public TagService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<Tag> Add(Tag entity)
        {
            return await _tagRepository.Add(entity);
        }

        public async Task Delete(Tag entity)
        {
            await _tagRepository.Delete(entity);
        }

        public async Task<Tag> Find(Guid Id)
        {
            return await _tagRepository.Find(Id);
        }

        public async Task<IEnumerable<Tag>> GetAll()
        {
            return await _tagRepository.GetAll();
        }

        public async Task SoftDelete(Tag entity)
        {
            entity.Deleted = true;
            await _tagRepository.Update(entity);
        }

        public async Task<Tag> Update(Tag entity)
        {
            return await _tagRepository.Update(entity);
        }
    }
}
