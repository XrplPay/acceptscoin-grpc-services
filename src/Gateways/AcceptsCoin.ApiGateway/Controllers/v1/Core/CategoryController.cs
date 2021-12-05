using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.ApiGateway.Core.Dtos;
using AcceptsCoin.ApiGateway.Core.Views;
using AcceptsCoin.Services.CoreServer;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcceptsCoin.ApiGateway.Controllers.v1.Core
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v1/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoryController : ControllerBase
    {
        public CategoryController()
        {

        }

        [HttpGet("GetCategoryList")]
        public async Task<ActionResult> GetCategoryList([FromQuery] int pageId = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var channel = GrpcChannel.ForAddress("https://localhost:5052");
                var client = new CategoryAppService.CategoryAppServiceClient(channel);
                var reply = await client.GetAllAsync(new Empty());

                return Ok(reply);
            }
            catch (Exception ex)
            {
                return BadRequest(new WebApiErrorMessageResponse()
                {
                    Errors = new List<string>() {
                            ex.Message
                    },
                    Success = false
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            try
            {
                var channel = GrpcChannel.ForAddress("https://localhost:5052");
                var client = new CategoryAppService.CategoryAppServiceClient(channel);
                var reply = await client.GetByIdAsync(new CategoryIdFilter { CategoryId = id.ToString() });

                return Ok(reply);
            }
            catch (Exception ex)
            {
                return BadRequest(new WebApiErrorMessageResponse()
                {
                    Errors = new List<string>() {
                            ex.Message
                    },
                    Success = false
                });
            }
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateCategoryDto createCategory)
        {
            try
            {
                var channel = GrpcChannel.ForAddress("https://localhost:5052");
                var client = new CategoryAppService.CategoryAppServiceClient(channel);
                var reply = await client.PostAsync(new CategoryGm { CategoryId = "", Name = createCategory.Name, Icon = createCategory.Icon
                    , Logo = createCategory.Logo, Priority = createCategory.Priority });

                return Ok(reply);
            }
            catch (Exception ex)
            {
                return BadRequest(new WebApiErrorMessageResponse()
                {
                    Errors = new List<string>() {
                            ex.Message
                    },
                    Success = false
                });
            }

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] UpdateCategoryDto updateCategory)
        {
            try
            {
                var channel = GrpcChannel.ForAddress("https://localhost:5052");
                var client = new CategoryAppService.CategoryAppServiceClient(channel);
                var reply = await client.PutAsync(new CategoryGm
                {
                    CategoryId = id.ToString(),
                    Name = updateCategory.Name,
                    Icon = updateCategory.Icon
                    ,
                    Logo = updateCategory.Logo,
                    Priority = updateCategory.Priority
                });

                return Ok(reply);
            }
            catch (Exception ex)
            {
                return BadRequest(new WebApiErrorMessageResponse()
                {
                    Errors = new List<string>() {
                            ex.Message
                    },
                    Success = false
                });
            }

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                var channel = GrpcChannel.ForAddress("https://localhost:5052");
                var client = new CategoryAppService.CategoryAppServiceClient(channel);
                var reply = await client.DeleteAsync(new CategoryIdFilter { CategoryId = id.ToString() });

                return Ok(reply);
            }
            catch (Exception ex)
            {
                return BadRequest(new WebApiErrorMessageResponse()
                {
                    Errors = new List<string>() {
                            ex.Message
                    },
                    Success = false
                });
            }

        }
    }
}
