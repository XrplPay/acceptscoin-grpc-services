using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.ApiGateway.Core.Dtos;
using AcceptsCoin.ApiGateway.Core.Dtos.Directory;
using AcceptsCoin.ApiGateway.Core.Views;
using AcceptsCoin.Services.DirectoryServer.Protos;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AcceptsCoin.ApiGateway.Controllers.v1.Directory
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v1/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BusinessController : Controller
    {
        const string channelUrl = "http://localhost:5053";

        public BusinessController()
        {

        }
        private Metadata GetHeader()
        {
            var accessToken = Request.Headers[HeaderNames.Authorization];

            var header = new Metadata();
            header.Add("Authorization", accessToken);
            return header;
        }


        [AllowAnonymous]
        [HttpGet("GetFrontBusinessList")]
        public async Task<ActionResult> GetFrontBusinessList([FromQuery] int pageId = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new BusinessAppService.BusinessAppServiceClient(channel);
                var reply = await client.GetFrontBusinessListAsync(new BusinessFrontQueryFilter { PageId = pageId, PageSize = pageSize }, headers: GetHeader());

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
        [HttpGet("GetAll")]
        public async Task<ActionResult> GetAll([FromQuery] int pageId = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new BusinessAppService.BusinessAppServiceClient(channel);
                var reply = await client.GetAllAsync(new BusinessQueryFilter { PageId = pageId, PageSize = pageSize }, headers: GetHeader());

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
        [HttpGet("GetByUserId")]
        public async Task<ActionResult> GetByUserId([FromQuery] int pageId = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new BusinessAppService.BusinessAppServiceClient(channel);
                var reply = await client.GetByUserIdAsync(new BusinessQueryFilter { PageId = pageId, PageSize = pageSize }, headers: GetHeader());

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
        public async Task<ActionResult> GetByPartnerId([FromQuery] Guid partnerId, [FromQuery] int pageId = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new BusinessAppService.BusinessAppServiceClient(channel);
                var reply = await client.GetByPartnerIdAsync(new BusinessPartnerIdQueryFilter { PartnerId = partnerId.ToString(), PageId = pageId, PageSize = pageSize }, headers: GetHeader());

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
                var client = new BusinessAppService.BusinessAppServiceClient(channel);
                var reply = await client.GetByIdAsync(new BusinessIdFilter { BusinessId = id.ToString() }, headers: GetHeader());

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
        public async Task<ActionResult> Post([FromBody] CreateBusinessDto entity)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new BusinessAppService.BusinessAppServiceClient(channel);
                var reply = await client.PostAsync(new BusinessGm
                {
                    Id = "",
                    Address = entity.Address,
                    CategoryId = entity.CategoryId.ToString(),
                    ContactNumber = entity.ContactNumber,
                    Description = entity.Description,
                    Email = entity.Email,
                    FaceBook = entity.FaceBook,
                    Instagram = entity.Instagram,
                    Latitude = entity.Latitude,
                    Longitude = entity.Longitude,
                    Manager = entity.Manager,
                    Name = entity.Name,
                    OfferedServices = entity.OfferedServices,
                    Owner = entity.Owner,
                    Twitter = entity.Twitter,
                    Verified = entity.Verified,
                    WebSiteUrl = entity.WebSiteUrl,

                }, headers: GetHeader()); ;

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
        public async Task<ActionResult> Put(Guid id, [FromBody] CreateBusinessDto entity)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new BusinessAppService.BusinessAppServiceClient(channel);
                var reply = await client.PutAsync(new BusinessGm
                {
                    Id = id.ToString(),
                    Address = entity.Address,
                    CategoryId = entity.CategoryId.ToString(),
                    ContactNumber = entity.ContactNumber,
                    Description = entity.Description,
                    Email = entity.Email,
                    FaceBook = entity.FaceBook,
                    Instagram = entity.Instagram,
                    Latitude = entity.Latitude,
                    Longitude = entity.Longitude,
                    Manager = entity.Manager,
                    Name = entity.Name,
                    OfferedServices = entity.OfferedServices,
                    Owner = entity.Owner,
                    Twitter = entity.Twitter,
                    Verified = entity.Verified,
                    WebSiteUrl = entity.WebSiteUrl,
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
                var client = new BusinessAppService.BusinessAppServiceClient(channel);
                var reply = await client.DeleteAsync(new BusinessIdFilter { BusinessId = id.ToString() }, headers: GetHeader());

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

        [HttpDelete("SoftDelete/{id}")]
        public async Task<ActionResult> SoftDelete(Guid id)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new BusinessAppService.BusinessAppServiceClient(channel);
                var reply = await client.SoftDeleteAsync(new BusinessIdFilter { BusinessId = id.ToString() }, headers: GetHeader());

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
                var client = new BusinessAppService.BusinessAppServiceClient(channel);

                BusinessDeleteCollectionGm collectionGm = new BusinessDeleteCollectionGm();
                foreach (var item in items.Items)
                {
                    collectionGm.Items.Add(new BusinessIdFilter { BusinessId = item.Id.ToString() });
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
