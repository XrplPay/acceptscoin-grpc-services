using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.ApiGateway.Core.Dtos;
using AcceptsCoin.ApiGateway.Core.Dtos.Identity;
using AcceptsCoin.ApiGateway.Core.Views;
using AcceptsCoin.Services.IdentityServer.Protos;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AcceptsCoin.ApiGateway.Controllers.v1.Identity
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v1/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RoleController : ControllerBase
    {
        const string channelUrl = "http://localhost:5051";
        public RoleController()
        {

        }



        [HttpGet("GetAll")]
        public async Task<ActionResult> GetAll([FromQuery] int pageId = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var accessToken = Request.Headers[HeaderNames.Authorization];

                //var header = new Metadata();
                //header.Add("Authorization", accessToken);
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new RoleAppService.RoleAppServiceClient(channel);
                var reply = await client.GetAllAsync(new RoleQueryFilter { PageId = pageId, PageSize = pageSize }, headers: GetHeader());

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
                var client = new RoleAppService.RoleAppServiceClient(channel);
                var reply = await client.GetByIdAsync(new RoleIdFilter { RoleId = id.ToString() }, headers: GetHeader());

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
        public async Task<ActionResult> Post([FromBody] CreateRoleDto entity)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new RoleAppService.RoleAppServiceClient(channel);
                var reply = await client.PostAsync(new RoleGm
                {
                    Id = "",
                    
                    Name = entity.Name,
                    

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

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] UpdateRoleDto entity)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new RoleAppService.RoleAppServiceClient(channel);
                var reply = await client.PutAsync(new RoleGm
                {
                    Id = id.ToString(),
                    
                    Name = entity.Name,
                    
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
                var client = new RoleAppService.RoleAppServiceClient(channel);
                var reply = await client.SoftDeleteAsync(new RoleIdFilter { RoleId = id.ToString() }, headers: GetHeader());

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
                var client = new RoleAppService.RoleAppServiceClient(channel);

                RoleDeleteCollectionGm collectionGm = new RoleDeleteCollectionGm();
                foreach (var item in items.Items)
                {
                    collectionGm.Items.Add(new RoleIdFilter { RoleId = item.Id.ToString() });
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
        private Metadata GetHeader()
        {
            var accessToken = Request.Headers[HeaderNames.Authorization];

            var header = new Metadata();
            header.Add("Authorization", accessToken);
            return header;
        }
    }
}
