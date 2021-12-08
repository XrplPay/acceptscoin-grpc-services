using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.ApiGateway.Core.Dtos;
using AcceptsCoin.ApiGateway.Core.Views;
using AcceptsCoin.Services.CoreServer ;
using AcceptsCoin.Services.CoreServer.Protos;
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
    public class LanguageController : ControllerBase
    {
        const string channelUrl = "https://localhost:5052";
        public LanguageController()
        {

        }

        [HttpGet("GetLanguageList")]
        public async Task<ActionResult> GetAll([FromQuery] int pageId = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new LanguageAppService.LanguageAppServiceClient(channel);
                var reply = await client.GetAllAsync(new EmptyLanguage());

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
                var client = new LanguageAppService.LanguageAppServiceClient(channel);
                var reply = await client.GetByIdAsync(new LanguageIdFilter { LanguageId = id.ToString() });

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
        public async Task<ActionResult> Post([FromBody] CreateLanguageDto entity)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new LanguageAppService.LanguageAppServiceClient(channel);
                var reply = await client.PostAsync(new LanguageGm
                {
                    LanguageId = "",
                    Name= entity.Name,
                    Code=entity.Code,
                    Icon=entity.Icon,
                    Logo=entity.Logo,
                    Priority=entity.Priority,
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

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] UpdateLanguageDto entity)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new LanguageAppService.LanguageAppServiceClient(channel);
                var reply = await client.PutAsync(new LanguageGm
                {
                    LanguageId = id.ToString(),
                    Name = entity.Name,
                    Code = entity.Code,
                    Icon = entity.Icon,
                    Logo = entity.Logo,
                    Priority = entity.Priority,
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
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new LanguageAppService.LanguageAppServiceClient(channel);
                var reply = await client.DeleteAsync(new LanguageIdFilter { LanguageId = id.ToString() });

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
