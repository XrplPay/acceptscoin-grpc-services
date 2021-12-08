using System;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.Services.CoreServer.Core.Interfaces;
using AcceptsCoin.Services.CoreServer.Domain.Models;
using AcceptsCoin.Services.CoreServer.Protos;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace AcceptsCoin.Services.CoreServer
{
    //[Authorize]
    public class TagGrpcService : TagAppService.TagAppServiceBase
    {
        private readonly ILogger<TagGrpcService> _logger;
        private ITagService _tagService;
        public TagGrpcService(ILogger<TagGrpcService> logger, ITagService tagService)
        {
            _logger = logger;
            _tagService = tagService;
        }

        public override async Task<TagListGm> GetAll(EmptyTag request, ServerCallContext context)
        {
            TagListGm response = new TagListGm();

            Console.WriteLine(context.GetHttpContext().User.Identity.Name);
            var categories = from prd in await _tagService.GetAll()
            select new TagGm()
            {
                TagId = prd.TagId.ToString(),
                Title = prd.Title,
                                        
            };
            response.Items.AddRange(categories.ToArray());
            return await Task.FromResult(response);
        }
     
        public override async Task<TagGm> GetById(TagIdFilter request,ServerCallContext context)
        {
            var Tag =await _tagService.Find(Guid.Parse(request.TagId));
            var searchedTag = new TagGm()
            {
               TagId=Tag.TagId.ToString(),
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
                CreatedById = Guid.Parse("999bb90f-3167-4f81-83bb-0c76d1d3ace5"),
                CreatedDate = DateTime.Now,
                Published = true,
                Deleted = false,
            };

            var res = await _tagService.Add(prdAdded);

            var response = new TagGm()
            {
                TagId = res.TagId.ToString(),
                Title = res.Title,
            };
            return await Task.FromResult(response);
        }


        public override async Task<TagGm> Put(TagGm request,
           ServerCallContext context)
        {
            Tag prd = await _tagService.Find(Guid.Parse(request.TagId));
            if (prd == null)
            {
                return await Task.FromResult<TagGm>(null);
            }


            prd.Title = request.Title;
            prd.UpdatedById = Guid.Parse("999bb90f-3167-4f81-83bb-0c76d1d3ace5");
            prd.UpdatedDate = DateTime.Now;
            




            await _tagService.Update(prd);
            return await Task.FromResult<TagGm>(new TagGm()
            {
                TagId = prd.TagId.ToString(),
                Title = prd.Title,
            });
        }


        
        public override async Task<EmptyTag> Delete(TagIdFilter request, ServerCallContext context)
        {
            Tag prd = await _tagService.Find(Guid.Parse(request.TagId));
            if (prd == null)
            {
                return await Task.FromResult<EmptyTag>(null);
            }

            await _tagService.Delete(prd);
            return await Task.FromResult<EmptyTag>(new EmptyTag());
        }
    }
}
