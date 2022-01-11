using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.Services.CoreServer.Domain.Interfaces;
using AcceptsCoin.Services.CoreServer.Domain.Models;
using Grpc.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace AcceptsCoin.Services.CoreServer
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoryService : CategoryAppService.CategoryAppServiceBase
    {
        private readonly ILogger<CategoryService> _logger;
        private ICategoryRepository _categoryRepository;
        public CategoryService(ILogger<CategoryService> logger, ICategoryRepository categoryRepository)
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
        }

        private Guid getUserId(ServerCallContext context)
        {
            return Guid.Parse(context.GetHttpContext().User.Identity.Name);
        }
        private string getPartnetId(ServerCallContext context)
        {
            return "bff3b2dd-e89d-46fc-a868-aab93a3efbbe";
        }
        public override async Task<CategoryListGm> GetAll(CategoryQueryFilter request, ServerCallContext context)
        {
            CategoryListGm response = new CategoryListGm();

            IQueryable<Category> query = _categoryRepository.GetQuery();


            response.CurrentPage = request.PageId;
            response.ItemCount = await _categoryRepository.GetCount(query);
            response.PageCount = (response.ItemCount / request.PageSize) + 1;

            var categories = from prd in await _categoryRepository.GetAll()
            select new CategoryGm()
            {
                Id = prd.CategoryId.ToString(),
                Icon = prd.Icon,
                Logo = prd.Logo,
                Name = prd.Name,
                Priority = prd.Priority,
            };

            response.Items.AddRange(categories.ToArray());
            return await Task.FromResult(response);

        }
        public override async Task<CategoryGm> GetById(CategoryIdFilter request,ServerCallContext context)
        {
            var category =await _categoryRepository.Find(request.CategoryId);
            var searchedCategory = new CategoryGm()
            {
               Id=category.CategoryId.ToString(),
               Icon=category.Icon,
               Name=category.Name,
               Logo=category.Logo,
               Priority=category.Priority,
            };
            return await Task.FromResult(searchedCategory);
        }

        public override async Task<CategoryGm> Post(CategoryGm request, ServerCallContext context)
        {
            var prdAdded = new Category()
            {
                CategoryId = Guid.NewGuid(),
                Name = request.Name,
                Icon = request.Icon,
                Logo = request.Logo,
                Priority = request.Priority,
                CreatedById = getUserId(context),
                CreatedDate = DateTime.Now,
                Published = true,
            };

            var res = await _categoryRepository.Add(prdAdded);

            var response = new CategoryGm()
            {
                Id = res.CategoryId.ToString(),
                Name = res.Name,
                Icon = res.Icon,
                Logo = res.Logo,
                Priority = res.Priority,

            };
            return await Task.FromResult(response);
        }


        public override async Task<CategoryGm> Put(CategoryGm request,
           ServerCallContext context)
        {
            Category prd = await _categoryRepository.Find(request.Id);
            if (prd == null)
            {
                return await Task.FromResult<CategoryGm>(null);
            }


            prd.Name = request.Name;
            prd.Logo = request.Logo;
            prd.Icon = request.Icon;
            prd.Priority = request.Priority;
            prd.UpdatedById = getUserId(context);
            prd.UpdatedDate = DateTime.Now;




            await _categoryRepository.Update(prd);
            return await Task.FromResult<CategoryGm>(new CategoryGm()
            {
                Id = prd.CategoryId.ToString(),
                Icon = prd.Icon,
                Logo = prd.Logo,
                Name = prd.Name,
                Priority = prd.Priority,
            });
        }

        public override async Task<Empty> Delete(CategoryIdFilter request, ServerCallContext context)
        {
            Category prd = await _categoryRepository.Find(request.CategoryId);
            if (prd == null)
            {
                return await Task.FromResult<Empty>(null);
            }

            await _categoryRepository.Delete(prd);
            return await Task.FromResult<Empty>(new Empty());
        }

        public override async Task<Empty> SoftDelete(CategoryIdFilter request, ServerCallContext context)
        {
            Category category = await _categoryRepository.Find(request.CategoryId);

            if (category == null)
            {
                return await Task.FromResult<Empty>(null);
            }

            category.Deleted = true;
            category.UpdatedById = getUserId(context);
            category.UpdatedDate = DateTime.Now;

            await _categoryRepository.Update(category);
            return await Task.FromResult<Empty>(new Empty());
        }
        public override async Task<Empty> SoftDeleteCollection(CategoryDeleteCollectionGm request, ServerCallContext context)
        {

            foreach (var item in request.Items)
            {
                Category category = await _categoryRepository.Find(item.CategoryId);

                if (category == null)
                {
                    return await Task.FromResult<Empty>(null);
                }

                category.Deleted = true;
                category.UpdatedById = getUserId(context);
                category.UpdatedDate = DateTime.Now;

                await _categoryRepository.Update(category);
            }

            return await Task.FromResult<Empty>(new Empty());
        }
    }
}
