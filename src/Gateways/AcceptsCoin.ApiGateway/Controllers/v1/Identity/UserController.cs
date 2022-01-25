using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.ApiGateway.Core.Dtos;
using AcceptsCoin.ApiGateway.Core.Dtos.Identity;
using AcceptsCoin.ApiGateway.Core.Views;
using AcceptsCoin.Services.DirectoryServer.Protos;
using AcceptsCoin.Services.IdentityServer.Protos;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;


namespace AcceptsCoin.ApiGateway.Controllers.v1.Identity
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v1/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        const string directoryChannelUrl = "http://localhost:5053";
        const string channelUrl = "http://localhost:5051";
        public UserController()
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
                var client = new UserAppService.UserAppServiceClient(channel);
                var reply = await client.GetAllAsync(new UserQueryFilter { PageId = pageId, PageSize = pageSize }, headers: GetHeader());

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

        [HttpGet("GetByPartnerId")]
        public async Task<ActionResult> GetByPartnerId([FromQuery] Guid partnerId , [FromQuery] int pageId = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var accessToken = Request.Headers[HeaderNames.Authorization];

                //var header = new Metadata();
                //header.Add("Authorization", accessToken);
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new UserAppService.UserAppServiceClient(channel);
                var reply = await client.GetByPartnerIdAsync(new UserPartnerIdQueryFilter {PartnerId= partnerId.ToString(), PageId = pageId, PageSize = pageSize }, headers: GetHeader());

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

        [HttpGet("GetByRoleId")]
        public async Task<ActionResult> GetByRoleId([FromQuery] Guid roleId, [FromQuery] int pageId = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var accessToken = Request.Headers[HeaderNames.Authorization];

                //var header = new Metadata();
                //header.Add("Authorization", accessToken);
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new UserAppService.UserAppServiceClient(channel);
                var reply = await client.GetByRoleIdAsync(new UserRoleIdQueryFilter { RoleId = roleId.ToString(), PageId = pageId, PageSize = pageSize }, headers: GetHeader());

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

        [HttpPost("SaveUserRole")]
        public async Task<ActionResult> SaveUserRole([FromBody] UserRoleDto  userRole)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new UserAppService.UserAppServiceClient(channel);
                await client.SaveUserRoleAsync(new UserRoleGm { UserId = userRole.UserId.ToString(), RoleId = userRole.RoleId.ToString() }, headers: GetHeader());

                return Ok();
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
                var client = new UserAppService.UserAppServiceClient(channel);
                var reply = await client.GetByIdAsync(new UserIdFilter { UserId = id.ToString() }, headers: GetHeader());

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

        [HttpGet("GetProfile")]
        public async Task<ActionResult> GetProfile()
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new UserAppService.UserAppServiceClient(channel);
                var reply = await client.GetProfileAsync(new EmptyUser(), headers: GetHeader());

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
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateUserDto entity)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new UserAppService.UserAppServiceClient(channel);
                var reply = await client.PostAsync(new CreateUserGm
                {
                    Id = "",
                    Email = entity.Email,
                    Name = entity.Name,
                    Password = entity.Password,

                });



                #region Dependency
                if (reply != null && reply.Id != null)
                {
                    var directoryChannel = GrpcChannel.ForAddress(directoryChannelUrl);
                    var directoryClient = new DirectoryAppService.DirectoryAppServiceClient(directoryChannel);
                    directoryClient.DirectoryUserPost(new DirectoryUserGm
                    {
                        Id = reply.Id,
                        Email = entity.Email,
                        Name = entity.Name,

                    });
                }
                #endregion


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
        public async Task<ActionResult> Put(Guid id, [FromBody] UpdateUserDto entity)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new UserAppService.UserAppServiceClient(channel);
                var reply = await client.PutAsync(new UserGm
                {
                    Id = id.ToString(),
                    Email = entity.Email,
                    Name = entity.Name,
                    Password = entity.Password,
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
                var client = new UserAppService.UserAppServiceClient(channel);
                var reply = await client.SoftDeleteAsync(new UserIdFilter { UserId = id.ToString() }, headers: GetHeader());

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
                var client = new UserAppService.UserAppServiceClient(channel);

                UserDeleteCollectionGm collectionGm = new UserDeleteCollectionGm();
                foreach (var item in items.Items)
                {
                    collectionGm.Items.Add(new UserIdFilter { UserId = item.Id.ToString() });
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
