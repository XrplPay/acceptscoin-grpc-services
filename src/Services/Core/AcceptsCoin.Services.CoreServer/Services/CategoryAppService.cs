using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.Services.CoreServer.Domain.Interfaces;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace AcceptsCoin.Services.CoreServer
{
    public class CategoryService : CategoryAppService.CategoryAppServiceBase
    {
        private readonly ILogger<CategoryService> _logger;
        private ICategoryRepository _categoryRepository;
        public CategoryService(ILogger<CategoryService> logger, ICategoryRepository categoryRepository)
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
        }

        public override async Task<CategoriesApp> GetAllCategory(Empty request, ServerCallContext context)
        {
            CategoriesApp response = new CategoriesApp();

            var categories = from prd in await _categoryRepository.GetAllCategory()
                             select new CategoryApp()
                             {
                                 CategoryId = prd.CategoryId.ToString(),
                                 Icon = prd.Icon,
                                 Logo = prd.Logo,
                                 Name = prd.Name,
                                 Priority = prd.Priority.ToString(),
                             };

            response.Items.AddRange(categories.ToArray());
            return await Task.FromResult(response);

        }
    }
}
