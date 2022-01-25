using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AcceptsCoin.ApiGateway.Core.Dtos.Directory;
using AcceptsCoin.ApiGateway.Core.Views;
using AcceptsCoin.Services.DirectoryServer.Protos;
using AcceptsCoin.Services.StorageServer.Protos;
using Google.Protobuf;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace AcceptsCoin.ApiGateway.Controllers.v1.Directory
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v1/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BusinessGalleryController : Controller
    {
        const string channelUrlStorage = "http://localhost:5054";
        const string channelUrl = "http://localhost:5053";
        public BusinessGalleryController()
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
        [HttpGet("GetBusinessGalleList")]
        public async Task<ActionResult> GetFrontBusinessList([FromQuery] Guid businessId, [FromQuery] int pageId = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new BusinessGalleryAppService.BusinessGalleryAppServiceClient(channel);
                var reply = await client.GetByBusinessIdAsync(new BusinessGalleryBusinessIdFilter { BusienssId = businessId.ToString(), PageId = pageId, PageSize = pageSize });

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



        [HttpGet("GetBusinessGalleryListByBusinessId")]
        public async Task<ActionResult> GetBusinessGalleryListByBusinessId([FromQuery] Guid businessId, [FromQuery] int pageId = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new BusinessGalleryAppService.BusinessGalleryAppServiceClient(channel);
                var reply = await client.GetByBusinessIdAsync(new BusinessGalleryBusinessIdFilter { BusienssId = businessId.ToString(), PageId = pageId, PageSize = pageSize });

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
                var client = new BusinessGalleryAppService.BusinessGalleryAppServiceClient(channel);
                var reply = await client.SoftDeleteAsync(new  BusinessGalleryIdFilter {  BusinessGalleryId = id.ToString() }, headers: GetHeader());

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




        public class FileModel
        {
            public string Extension { get; set; }
            public string Name { get; set; }
            public string Url { get; set; }
        }

        private async Task<FileModel> Upload(IFormFile file)
        {
            var channel = GrpcChannel.ForAddress(channelUrlStorage);
            var client = new UploadFileAppService.UploadFileAppServiceClient(channel);
            FileModel fileModel = new FileModel();
            var fileUrl = "";

            if (file.Length > 0)
            {
                var index = file.FileName.IndexOf(".");
                var extension = file.FileName.Substring(index, file.FileName.Length - index);
                var newName = Guid.NewGuid().ToString();
                fileUrl = newName + extension;

                fileModel = new FileModel { Extension = extension, Name = newName, Url = fileUrl };

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






            return fileModel;
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromForm] CreateBusinessGalleryDto entity)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new BusinessAppService.BusinessAppServiceClient(channel);
                var galleryClient = new BusinessGalleryAppService.BusinessGalleryAppServiceClient(channel);


                List<FileModel> fileModels = new List<FileModel>();

                if (entity.Files != null)
                {
                    foreach (var file in entity.Files.Files)
                    {
                        fileModels.Add(await Upload(file));
                    }
                }

                foreach (var item in fileModels)
                {
                    galleryClient.Post(new BusinessGalleryGm
                    {
                        Extension = item.Extension,
                        Id = "",
                        Name = item.Name,
                        BusinessId = entity.BusinessId.ToString(),
                    }, headers: GetHeader());
                }
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

    }
}
