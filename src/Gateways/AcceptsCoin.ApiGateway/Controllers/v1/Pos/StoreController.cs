using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AcceptsCoin.ApiGateway.Core.Dtos;
using AcceptsCoin.ApiGateway.Core.Views;
using AcceptsCoin.ApiGateway.Helper;
using AcceptsCoin.Services.PosServer.Protos;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;


namespace AcceptsCoin.ApiGateway.Controllers.v1.Pos
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v1/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class StoreController : Controller
    {
        const string IdentityChannelUrl = "http://localhost:5051";
        const string channelUrl = "http://localhost:5056";
        private IDataControl _dataControl;
        public StoreController(IDataControl dataControl)
        {
            _dataControl = dataControl;
        }
        private Metadata GetHeader()
        {
            var accessToken = Request.Headers[HeaderNames.Authorization];

            var header = new Metadata();
            header.Add("Authorization", accessToken);
            return header;
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] int pageId = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var accessToken = Request.Headers[HeaderNames.Authorization];
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new StoreAppService.StoreAppServiceClient(channel);
                var reply = await client.GetAllAsync(new StoreQueryFilter { PageId = pageId, PageSize = pageSize }, headers: GetHeader());

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
                var client = new StoreAppService.StoreAppServiceClient(channel);
                var reply = await client.GetByIdAsync(new StoreIdFilter { StoreId = id.ToString() }, headers: GetHeader());

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
        public async Task<ActionResult> Post([FromBody] CreateStoreDto entity)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new StoreAppService.StoreAppServiceClient(channel);
                var reply = await client.PostAsync(new CreateStoreGm
                {
                    Threshold = entity.Threshold,
                    RefundDay = entity.RefundDay,
                    Name = entity.Name,
                    WebSite = entity.WebSite,
                    Email = entity.Email,
                    DefaultCurrency = entity.DefaultCurrency,

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

        [HttpPost("WoocommercePost")]
        public async Task<ActionResult> WoocommercePost([FromBody] WoocommerceCreateStoreDto entity)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new StoreAppService.StoreAppServiceClient(channel);
                var reply = await client.WoocommercePostAsync(new WoocommerceCreateStoreGm
                {
                    Name = entity.Name,
                    WebSite = entity.WebSite,
                    Email = entity.Email,
                    DefaultCurrency = entity.DefaultCurrency,
                    UserId = "",

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
        public async Task<ActionResult> Put(Guid id, [FromBody] UpdateStoreDto entity)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new StoreAppService.StoreAppServiceClient(channel);
                var reply = await client.PutAsync(new UpdateStoreGm
                {
                    Id = id.ToString(),
                    DefaultCurrency = entity.DefaultCurrency,
                    Name = entity.Name,
                    RefundDay = entity.RefundDay,
                    Threshold = entity.Threshold,
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
                var client = new StoreAppService.StoreAppServiceClient(channel);
                var reply = await client.SoftDeleteAsync(new StoreIdFilter { StoreId = id.ToString() }, headers: GetHeader());

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
                var client = new StoreAppService.StoreAppServiceClient(channel);

                StoreDeleteCollectionGm collectionGm = new StoreDeleteCollectionGm();
                foreach (var item in items.Items)
                {
                    collectionGm.Items.Add(new StoreIdFilter { StoreId = item.Id.ToString() });
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
