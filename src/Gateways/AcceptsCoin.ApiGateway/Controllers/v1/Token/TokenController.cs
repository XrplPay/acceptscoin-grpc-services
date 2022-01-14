using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.ApiGateway.Core.Dtos;
using AcceptsCoin.ApiGateway.Core.Views;
using AcceptsCoin.Services.TokenServer ;
using AcceptsCoin.Services.TokenServer.Protos;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace AcceptsCoin.ApiGateway.Controllers.v1.Token
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v1/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TokenController : ControllerBase
    {
        const string channelUrl = "http://localhost:5055";
        public TokenController()
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
                var client = new TokenAppService.TokenAppServiceClient(channel);
                var reply = await client.GetAllAsync(new TokenQueryFilter { PageId = pageId, PageSize = pageSize }, headers: GetHeader());

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
        [HttpGet("GetFrontTokenList")]
        public async Task<ActionResult> GetFrontTokenList()
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new TokenAppService.TokenAppServiceClient(channel);
                var reply = await client.GetFrontTokenListAsync(new Empty());

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
                var client = new TokenAppService.TokenAppServiceClient(channel);
                var reply = await client.GetByIdAsync(new TokenIdFilter { TokenId = id.ToString() }, headers: GetHeader());

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
        public async Task<ActionResult> Post([FromBody] CreateTokenDto entity)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new TokenAppService.TokenAppServiceClient(channel);
                var reply = await client.PostAsync(new TokenGm
                {
                    Id = "",
                    Name = entity.Name,
                    Symbol = entity.Symbol,
                    Description = entity.Description,

                    Icon = "",

                    Logo = entity.Logo,
                    Priority = entity.Priority,
                    Link = entity.Link,
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
        public async Task<ActionResult> Put(Guid id, [FromBody] UpdateTokenDto entity)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new TokenAppService.TokenAppServiceClient(channel);
                var reply = await client.PutAsync(new TokenGm
                {
                    Id = id.ToString(),
                    Name = entity.Name,
                    Symbol = entity.Symbol,
                    Description = entity.Description,

                    Icon = entity.Icon,

                    Logo = entity.Logo,
                    Priority = entity.Priority,
                    Link = entity.Link,
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
                var client = new TokenAppService.TokenAppServiceClient(channel);
                var reply = await client.SoftDeleteAsync(new TokenIdFilter { TokenId = id.ToString() }, headers: GetHeader());

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
                var client = new TokenAppService.TokenAppServiceClient(channel);

                DeleteCollectionGm collectionGm = new DeleteCollectionGm();
                foreach (var item in items.Items)
                {
                    collectionGm.Items.Add(new TokenIdFilter { TokenId = item.Id.ToString() });
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
