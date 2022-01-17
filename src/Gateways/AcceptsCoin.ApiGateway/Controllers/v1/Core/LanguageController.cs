using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.ApiGateway.Core.Dtos;
using AcceptsCoin.ApiGateway.Core.Views;
using AcceptsCoin.Services.CoreServer ;
using AcceptsCoin.Services.CoreServer.Protos;
using AcceptsCoin.Services.StorageServer.Protos;
using Google.Protobuf;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace AcceptsCoin.ApiGateway.Controllers.v1.Core
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v1/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LanguageController : ControllerBase
    {
        const string channelUrlStorage = "http://localhost:5054";
        const string channelUrl = "http://localhost:5052";
        public LanguageController()
        {

        }
        private Metadata GetHeader()
        {
            var accessToken = Request.Headers[HeaderNames.Authorization];

            var header = new Metadata();
            header.Add("Authorization", accessToken);
            return header;
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
        [HttpGet("GetAll")]
        public async Task<ActionResult> GetAll([FromQuery] int pageId = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new LanguageAppService.LanguageAppServiceClient(channel);
                var reply = await client.GetAllAsync(new LanguageQueryFilter { PageId = pageId, PageSize = pageSize }, headers: GetHeader());

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
                var reply = await client.GetByIdAsync(new LanguageIdFilter { LanguageId = id.ToString() }, headers: GetHeader());

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
        public async Task<ActionResult> Post([FromForm] CreateLanguageDto entity)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new LanguageAppService.LanguageAppServiceClient(channel);
                var logo = await Upload(entity.File);
                var reply = await client.PostAsync(new LanguageGm
                {
                    Id = "",
                    Name = entity.Name,
                    Code = entity.Code,
                    Icon = entity.Icon,
                    Logo = logo,
                    Priority = entity.Priority,
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
        public async Task<ActionResult> Put(Guid id, [FromBody] UpdateLanguageDto entity)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new LanguageAppService.LanguageAppServiceClient(channel);
                var reply = await client.PutAsync(new LanguageGm
                {
                    Id = id.ToString(),
                    Name = entity.Name,
                    Code = entity.Code,
                    Icon = entity.Icon,
                    Logo = entity.Logo,
                    Priority = entity.Priority,
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
                var client = new LanguageAppService.LanguageAppServiceClient(channel);
                var reply = await client.SoftDeleteAsync(new LanguageIdFilter { LanguageId = id.ToString() }, headers: GetHeader());

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
                var client = new LanguageAppService.LanguageAppServiceClient(channel);

                LanguageDeleteCollectionGm collectionGm = new  LanguageDeleteCollectionGm();
                foreach (var item in items.Items)
                {
                    collectionGm.Items.Add(new LanguageIdFilter { LanguageId = item.Id.ToString() });
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
