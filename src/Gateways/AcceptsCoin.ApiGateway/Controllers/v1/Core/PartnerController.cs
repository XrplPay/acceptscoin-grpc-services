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
    public class PartnerController : ControllerBase
    {

        const string channelUrl = "http://localhost:5052";

        public PartnerController()
        {

        }

        [HttpGet("GetAllo")]
        public async Task<ActionResult> GetAll([FromQuery] int pageId = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new PartnerAppService.PartnerAppServiceClient(channel);
                var reply = await client.GetAllAsync(new EmptyPartner());

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
                var client = new PartnerAppService.PartnerAppServiceClient(channel);
                var reply = await client.GetByIdAsync(new PartnerIdFilter { PartnerId = id.ToString() });

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
        public async Task<ActionResult> Post([FromBody] CreatePartnerDto entity)
        {
            try

            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new PartnerAppService.PartnerAppServiceClient(channel);
                var reply = await client.PostAsync(new PartnerGm
                {
                    PartnerId = "",
                    Name = entity.Name,
                    ContactNumber = entity.ContactNumber,
                    Email = entity.Email,
                    LanguageId = entity.LanguageId.ToString(),
                    Logo = entity.Logo,
                    Manager = entity.Manager,
                    Owner = entity.Owner,
                    WebSiteUrl = entity.WebSiteUrl,
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
        public async Task<ActionResult> Put(Guid id, [FromBody] UpdatePartnerDto entity)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new PartnerAppService.PartnerAppServiceClient(channel);
                var reply = await client.PutAsync(new PartnerGm
                {
                    PartnerId = id.ToString(),
                    Name = entity.Name,
                    ContactNumber = entity.ContactNumber,
                    Email = entity.Email,
                    LanguageId = entity.LanguageId.ToString(),
                    Logo = entity.Logo,
                    Manager = entity.Manager,
                    Owner = entity.Owner,
                    WebSiteUrl = entity.WebSiteUrl,
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
                var client = new PartnerAppService.PartnerAppServiceClient(channel);
                var reply = await client.DeleteAsync(new PartnerIdFilter { PartnerId = id.ToString() });

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
