using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.ApiGateway.Core.Views;
using AcceptsCoin.Services.CoreServer;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AcceptsCoin.ApiGateway.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v1/[controller]")]
    public class CoreController : Controller
    {
        public CoreController()
        {

        }

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[CustomAuthorize(new string[] { AuthorizeRole.Administrator })]
        [HttpGet("GetCategoryList")]
        public async Task<ActionResult> GetCategoryList([FromQuery] int pageId = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var channel = GrpcChannel.ForAddress("https://localhost:9000");
                var client = new CategoryAppService.CategoryAppServiceClient(channel);
                var reply = await client.GetAllCategoryAsync(new Empty());

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
