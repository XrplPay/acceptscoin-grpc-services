using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.ApiGateway.Core.Dtos;
using AcceptsCoin.ApiGateway.Core.Views;
using AcceptsCoin.Services.CoreServer;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using AcceptsCoin.Services.CoreServer.Protos;
using System.IO;
using Google.Protobuf;
using AcceptsCoin.Services.StorageServer.Protos;
using Microsoft.AspNetCore.Http;

namespace AcceptsCoin.ApiGateway.Controllers.v1.Core
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v1/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoryController : ControllerBase
    {
        const string channelUrlStorage = "http://localhost:5054";
        const string channelUrl = "http://localhost:5052";
        public CategoryController()
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
                var client = new CategoryAppService.CategoryAppServiceClient(channel);
                var reply = await client.GetAllAsync(new CategoryQueryFilter { PageId = pageId, PageSize = pageSize }, headers: GetHeader());

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
                var client = new CategoryAppService.CategoryAppServiceClient(channel);
                var reply = await client.GetByPartnerIdAsync(new PartnerCategoryQueryFilter { PartnerId = partnerId.ToString(), PageId = pageId, PageSize = pageSize }, headers: GetHeader());

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

        [HttpPost("SavePartnerCategory")]
        public async Task<ActionResult> SavePartnerToken([FromBody] PartnerCategoryDto  partnerCategory)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new CategoryAppService.CategoryAppServiceClient(channel);
                await client.SavePartnerCategoryAsync(new PartnerCategoryGm { PartnerId = partnerCategory.PartnerId.ToString(), CategoryId = partnerCategory.CategoryId.ToString() }, headers: GetHeader());

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

        //private async Task<CategoryChildrenListGm> GetChild(string Id)
        //{

        //    var channel = GrpcChannel.ForAddress(channelUrl);
        //    var client = new CategoryAppService.CategoryAppServiceClient(channel);

        //    var categories = await client.GetChildAsync(new CategoryIdFilter { CategoryId = Id.ToString() }, headers: GetHeader());

        //    return categories;

        //}

        [AllowAnonymous]
        [HttpGet("GetTree")]
        public async Task<ActionResult> GetTree()
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new CategoryAppService.CategoryAppServiceClient(channel);
                var reply = await client.GetTreeAsync(new Empty { });

                //var categories = reply.Items.ToList();

                //foreach (var item in categories)
                //{
                //    var children = await GetChild(item.Id);

                //    categories.Where(x => x.Id == item.Id).First().Children.AddRange(children.Items.ToList());
                //}
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
        [HttpGet("GetMenu")]
        public async Task<ActionResult> GetMenu()
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new CategoryAppService.CategoryAppServiceClient(channel);
                var reply = await client.GetMenuAsync(new Empty { });

                //var categories = reply.Items.ToList();

                //foreach (var item in categories)
                //{
                //    var children = await GetChild(item.Id);

                //    categories.Where(x => x.Id == item.Id).First().Children.AddRange(children.Items.ToList());
                //}
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
        [HttpGet("GetFrontTree")]
        public async Task<ActionResult> GetFrontTree()
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new CategoryAppService.CategoryAppServiceClient(channel);
                var reply = await client.GetFrontTreeAsync(new Empty { });

              
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
                var client = new CategoryAppService.CategoryAppServiceClient(channel);
                var reply = await client.GetByIdAsync(new CategoryIdFilter { CategoryId = id.ToString() }, headers: GetHeader());

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

        private async Task<string> Upload(IFormFile file)
        {
            var userId = Guid.Parse("999bb90f-3167-4f81-83bb-0c76d1d3ace5");
            //var gallery = new Gallery();



            var channel = GrpcChannel.ForAddress(channelUrlStorage);
            var client = new UploadFileAppService.UploadFileAppServiceClient(channel);

            var fileUrl = "";

            if (file.Length > 0)
            {
                var index = file.FileName.IndexOf(".");
                var extension = file.FileName.Substring(index, file.FileName.Length - index);
                var newName = Guid.NewGuid().ToString();
                fileUrl = newName + extension;


                MemoryStream ms = new MemoryStream();
                await file.CopyToAsync(ms);

                var content = new BytesContent
                {
                    FileSize = ms.Length,
                    ReadedByte = 0,
                    Info = new FileInfod { FileName = newName, FileExtension = extension }

                };

                var upload = client.FileUpLoad(headers: GetHeader());

                ms.Position = 0;




                byte[] buffer = new byte[2048];

                while ((content.ReadedByte = await ms.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    content.Buffer = ByteString.CopyFrom(buffer);
                    await upload.RequestStream.WriteAsync(content);
                }
                await upload.RequestStream.CompleteAsync();

                file.OpenReadStream().Close();
            }

            return fileUrl;
        }
        [HttpPost]
        public async Task<ActionResult> Post([FromForm] CreateCategoryDto createCategory)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new CategoryAppService.CategoryAppServiceClient(channel);
                var logo = await Upload(createCategory.File);
                var reply = await client.PostAsync(new CategoryGm { Id = "", Name = createCategory.Name, Icon = createCategory.Icon
                    , Logo = logo, Priority = createCategory.Priority }, headers: GetHeader());

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
        public async Task<ActionResult> Put(Guid id, [FromBody] UpdateCategoryDto updateCategory)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new CategoryAppService.CategoryAppServiceClient(channel);
                var reply = await client.PutAsync(new CategoryGm
                {
                    Id = id.ToString(),
                    Name = updateCategory.Name,
                    Icon = updateCategory.Icon
                    ,
                    Logo = updateCategory.Logo,
                    Priority = updateCategory.Priority
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
                var client = new CategoryAppService.CategoryAppServiceClient(channel);
                var reply = await client.SoftDeleteAsync(new CategoryIdFilter { CategoryId = id.ToString() }, headers: GetHeader());

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
                var client = new CategoryAppService.CategoryAppServiceClient(channel);

                CategoryDeleteCollectionGm collectionGm = new CategoryDeleteCollectionGm();
                foreach (var item in items.Items)
                {
                    collectionGm.Items.Add(new CategoryIdFilter { CategoryId = item.Id.ToString() });
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
