using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.ApiGateway.Core.Dtos;
using AcceptsCoin.ApiGateway.Core.Views;
using AcceptsCoin.Services.CoreServer;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using AcceptsCoin.Services.CoreServer.Protos;

namespace AcceptsCoin.ApiGateway.Controllers.v1.Core
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v1/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoryController : ControllerBase
    {
        const string channelUrl = "http://localhost:5052";
        public CategoryController()
        {

        }
        private Metadata GetHeader()
        {
            var accessToken = Request.Headers[HeaderNames.Authorization];

            var header = new Metadata();
            header.Add("Authorization", accessToken);
            return header;
        }
        [HttpGet("GetAll")]
        public async Task<ActionResult> GetAll([FromQuery] int pageId = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new CategoryAppService.CategoryAppServiceClient(channel);
                var reply = await client.GetAllAsync(new CategoryQueryFilter { PageId = pageId, PageSize = pageSize }, headers: GetHeader());

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


        //private async Task<CategoryChildrenListGm> GetChild(string Id)
        //{

        //    var channel = GrpcChannel.ForAddress(channelUrl);
        //    var client = new CategoryAppService.CategoryAppServiceClient(channel);

        //    var categories = await client.GetChildAsync(new CategoryIdFilter { CategoryId = Id.ToString() }, headers: GetHeader());

        //    return categories;

        //}

        [HttpGet("GetTree")]
        public async Task<ActionResult> GetTree()
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new CategoryAppService.CategoryAppServiceClient(channel);
                var reply = await client.GetTreeAsync(new Empty { }, headers: GetHeader());

                //var categories = reply.Items.ToList();

                //foreach (var item in categories)
                //{
                //    var children = await GetChild(item.Id);

                //    categories.Where(x => x.Id == item.Id).First().Children.AddRange(children.Items.ToList());
                //}
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


        [AllowAnonymous]
        [HttpGet("GetFrontTree")]
        public async Task<ActionResult> GetFrontTree()
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new CategoryAppService.CategoryAppServiceClient(channel);
                var reply = await client.GetFrontTreeAsync(new Empty { });

              
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
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new CategoryAppService.CategoryAppServiceClient(channel);
                var reply = await client.GetByIdAsync(new CategoryIdFilter { CategoryId = id.ToString() }, headers: GetHeader());

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
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new CategoryAppService.CategoryAppServiceClient(channel);
                var reply = await client.PostAsync(new CategoryGm { Id = "", Name = createCategory.Name, Icon = createCategory.Icon
                    , Logo = createCategory.Logo, Priority = createCategory.Priority }, headers: GetHeader());

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
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new CategoryAppService.CategoryAppServiceClient(channel);
                var reply = await client.PutAsync(new CategoryGm
                {
                    Id = id.ToString(),
                    Name = updateCategory.Name,
                    Icon = updateCategory.Icon
                    ,
                    Logo = updateCategory.Logo,
                    Priority = updateCategory.Priority
                }, headers: GetHeader());

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
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new CategoryAppService.CategoryAppServiceClient(channel);
                var reply = await client.SoftDeleteAsync(new CategoryIdFilter { CategoryId = id.ToString() }, headers: GetHeader());

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
        [HttpDelete("DeleteCollection")]
        public async Task<ActionResult> DeleteCollection([FromBody] DeleteCollectionTokenDto items)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new CategoryAppService.CategoryAppServiceClient(channel);

                CategoryDeleteCollectionGm collectionGm = new CategoryDeleteCollectionGm();
                foreach (var item in items.Items)
                {
                    collectionGm.Items.Add(new CategoryIdFilter { CategoryId = item.Id.ToString() });
                }

                var reply = await client.SoftDeleteCollectionAsync(collectionGm, headers: GetHeader());

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
