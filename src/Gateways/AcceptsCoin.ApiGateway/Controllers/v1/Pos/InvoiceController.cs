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
    public class InvoiceController : Controller
    {
        const string channelUrl = "http://localhost:5056";
        private IDataControl _dataControl;
        public InvoiceController(IDataControl dataControl)
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
                var storeId = "";
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new InvoiceAppService.InvoiceAppServiceClient(channel);
                var reply = await client.GetAllAsync(new InvoiceQueryFilter { StoreId = storeId, PageId = pageId, PageSize = pageSize }, headers: GetHeader());

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
                var client = new InvoiceAppService.InvoiceAppServiceClient(channel);
                var reply = await client.GetByIdAsync(new InvoiceIdFilter {  InvoiceId = id.ToString() }, headers: GetHeader());

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
        public async Task<ActionResult> Post([FromBody] CreateInvoiceDto entity)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new InvoiceAppService.InvoiceAppServiceClient(channel);
                var reply = await client.PostAsync(new InvoiceGm
                {
                    Id = "",
                    Amount = entity.Amount,
                    StoreId = "",
                    NotificationUrl = entity.NotificationUrl,
                    NotificationEmail = entity.NotificationEmail,
                    Currency = entity.Currency,
                    CustomerEmail = entity.CustomerEmail,
                    OrderId = entity.OrderId.HasValue ? entity.OrderId.Value.ToString() : "",

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
        public async Task<ActionResult> Put(Guid id, [FromBody] UpdateInvoiceDto entity)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new InvoiceAppService.InvoiceAppServiceClient(channel);
                var reply = await client.PutAsync(new InvoiceGm
                {
                    Id = id.ToString(),
                    Amount = entity.Amount,
                    StoreId = "",
                    NotificationUrl = entity.NotificationUrl,
                    NotificationEmail = entity.NotificationEmail,
                    Currency = entity.Currency,
                    CustomerEmail = entity.CustomerEmail,
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
                var client = new InvoiceAppService.InvoiceAppServiceClient(channel);
                var reply = await client.SoftDeleteAsync(new InvoiceIdFilter { InvoiceId = id.ToString() }, headers: GetHeader());

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
                var client = new InvoiceAppService.InvoiceAppServiceClient(channel);

                InvoiceDeleteCollectionGm collectionGm = new InvoiceDeleteCollectionGm();
                foreach (var item in items.Items)
                {
                    collectionGm.Items.Add(new InvoiceIdFilter { InvoiceId = item.Id.ToString() });
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
