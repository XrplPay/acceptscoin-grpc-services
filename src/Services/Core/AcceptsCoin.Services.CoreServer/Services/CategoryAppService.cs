using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.Services.CoreServer.Domain.Interfaces;
using AcceptsCoin.Services.CoreServer.Domain.Models;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace AcceptsCoin.Services.CoreServer
{
    //[Authorize]
    public class CategoryService : CategoryAppService.CategoryAppServiceBase
    {
        private readonly ILogger<CategoryService> _logger;
        private ICategoryRepository _categoryRepository;
        public CategoryService(ILogger<CategoryService> logger, ICategoryRepository categoryRepository)
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
        }

        public override async Task<CategoryListGm> GetAll(Empty request, ServerCallContext context)
        {
            CategoryListGm response = new CategoryListGm();

            Console.WriteLine(context.GetHttpContext().User.Identity.Name);
            var categories = from prd in await _categoryRepository.GetAll()
            select new CategoryGm()
            {
                CategoryId = prd.CategoryId.ToString(),
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
               CategoryId=category.CategoryId.ToString(),
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
                CreatedById = Guid.Parse("999bb90f-3167-4f81-83bb-0c76d1d3ace5"),
                CreatedDate = DateTime.Now,
                Published = true,
            };

            var res = await _categoryRepository.Add(prdAdded);

            var response = new CategoryGm()
            {
                CategoryId = res.CategoryId.ToString(),
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
            Category prd = await _categoryRepository.Find(request.CategoryId);
            if (prd == null)
            {
                return await Task.FromResult<CategoryGm>(null);
            }


            prd.Name = request.Name;
            prd.Logo = request.Logo;
            prd.Icon = request.Icon;
            prd.Priority = request.Priority;
            prd.UpdatedById = Guid.Parse("999bb90f-3167-4f81-83bb-0c76d1d3ace5");
            prd.UpdatedDate = DateTime.Now;




            await _categoryRepository.Update(prd);
            return await Task.FromResult<CategoryGm>(new CategoryGm()
            {
                CategoryId = prd.CategoryId.ToString(),
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
    }
}
