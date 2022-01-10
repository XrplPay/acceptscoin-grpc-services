using AcceptsCoin.ApiGateway.Core.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Net.Client;
using AcceptsCoin.Services.IdentityServer.Protos;
using AcceptsCoin.ApiGateway.Core.Views;

namespace AcceptsCoin.ApiGateway.Controllers.v1.Identity
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        const string channelUrl = "http://localhost:5051";
        public AuthController()
        {

        }

        [HttpPost("Authenticate")]
        public async Task<ActionResult> Authenticate([FromBody] AuhenticateDto  auhenticateDto)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new AuthAppService.AuthAppServiceClient(channel);
                var reply = await client.AuthenticateAsync(new AuthGm
                {
                    
                    Username = auhenticateDto.UserName,
                    Password = auhenticateDto.Password
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
    }
}
