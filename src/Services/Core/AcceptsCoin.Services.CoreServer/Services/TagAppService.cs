using System;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.Services.CoreServer.Core.Interfaces;
using AcceptsCoin.Services.CoreServer.Domain.Interfaces;
using AcceptsCoin.Services.CoreServer.Domain.Models;
using AcceptsCoin.Services.CoreServer.Protos;
using Grpc.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace AcceptsCoin.Services.CoreServer
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TagGrpcService : TagAppService.TagAppServiceBase
    {
        private readonly ILogger<TagGrpcService> _logger;
        private ITagRepository  _tagRepository;
        public TagGrpcService(ILogger<TagGrpcService> logger, ITagRepository tagRepository)
        {
            _logger = logger;
            _tagRepository = tagRepository;
        }

        private Guid getUserId(ServerCallContext context)
        {
            return Guid.Parse(context.GetHttpContext().User.Identity.Name);
        }
        private string getPartnetId(ServerCallContext context)
        {
            return "bff3b2dd-e89d-46fc-a868-aab93a3efbbe";
        }
        public override async Task<TagListGm> GetAll(TagQueryFilter request, ServerCallContext context)
        {
            TagListGm response = new TagListGm();

            IQueryable<Tag> query = _tagRepository.GetQuery();


            response.CurrentPage = request.PageId;
            response.ItemCount = await _tagRepository.GetCount(query);
            response.PageCount = (response.ItemCount / request.PageSize) + 1;


            var tags = from tag in await _tagRepository.GetAll(query, request.PageId, request.PageSize)
            select new TagGm()
            {
                Id = tag.TagId.ToString(),
                Title = tag.Title,
                                        
            };
            response.Items.AddRange(tags.ToArray());
            return await Task.FromResult(response);
        }


        [AllowAnonymous]
        public override async Task<TagListFrontGm> GetAllTagFront(TagQueryFilter request, ServerCallContext context)
        {
            TagListFrontGm response = new TagListFrontGm();

            IQueryable<Tag> query = _tagRepository.GetQuery();


            response.CurrentPage = request.PageId;
            response.ItemCount = await _tagRepository.GetCount(query);
            response.PageCount = (response.ItemCount / request.PageSize) + 1;


            var tags = from tag in await _tagRepository.GetAll(query, request.PageId, request.PageSize)
                       select new TagFrontGm()
                       {
                           Id = tag.TagId.ToString(),
                           Title = tag.Title,

                       };
            response.Items.AddRange(tags.ToArray());
            return await Task.FromResult(response);
        }

        public override async Task<TagGm> GetById(TagIdFilter request,ServerCallContext context)
        {
            var Tag =await _tagRepository.Find(Guid.Parse(request.TagId));
            var searchedTag = new TagGm()
            {
               Id=Tag.TagId.ToString(),
               Title=Tag.Title,
               
            };
            return await Task.FromResult(searchedTag);
        }

        public override async Task<TagGm> Post(TagGm request, ServerCallContext context)
        {

            var prdAdded = new Tag()
            {
                TagId = Guid.NewGuid(),
                Title = request.Title,                
                CreatedById = getUserId(context),
                CreatedDate = DateTime.Now,
                Published = true,
                Deleted = false,
            };

            var res = await _tagRepository.Add(prdAdded);

            var response = new TagGm()
            {
                Id = res.TagId.ToString(),
                Title = res.Title,
            };
            return await Task.FromResult(response);
        }


        public override async Task<TagGm> Put(TagGm request,
           ServerCallContext context)
        {
            Tag prd = await _tagRepository.Find(Guid.Parse(request.Id));
            if (prd == null)
            {
                return await Task.FromResult<TagGm>(null);
            }


            prd.Title = request.Title;
            prd.UpdatedById = getUserId(context);
            prd.UpdatedDate = DateTime.Now;
            




            await _tagRepository.Update(prd);
            return await Task.FromResult<TagGm>(new TagGm()
            {
                Id = prd.TagId.ToString(),
                Title = prd.Title,
            });
        }


        
        public override async Task<EmptyTag> Delete(TagIdFilter request, ServerCallContext context)
        {
            Tag prd = await _tagRepository.Find(Guid.Parse(request.TagId));
            if (prd == null)
            {
                return await Task.FromResult<EmptyTag>(null);
            }

            await _tagRepository.Delete(prd);
            return await Task.FromResult<EmptyTag>(new EmptyTag());
        }

        public override async Task<EmptyTag> SoftDelete(TagIdFilter request, ServerCallContext context)
        {
            Tag tag = await _tagRepository.Find(Guid.Parse(request.TagId));

            if (tag == null)
            {
                return await Task.FromResult<EmptyTag>(null);
            }

            tag.Deleted = true;
            tag.UpdatedById = getUserId(context);
            tag.UpdatedDate = DateTime.Now;

            await _tagRepository.Update(tag);
            return await Task.FromResult<EmptyTag>(new EmptyTag());
        }
        public override async Task<EmptyTag> SoftDeleteCollection(TagDeleteCollectionGm request, ServerCallContext context)
        {

            foreach (var item in request.Items)
            {
                Tag tag = await _tagRepository.Find(Guid.Parse(item.TagId));

                if (tag == null)
                {
                    return await Task.FromResult<EmptyTag>(null);
                }

                tag.Deleted = true;
                tag.UpdatedById = getUserId(context);
                tag.UpdatedDate = DateTime.Now;

                await _tagRepository.Update(tag);
            }

            return await Task.FromResult<EmptyTag>(new EmptyTag());
        }
    }
}
