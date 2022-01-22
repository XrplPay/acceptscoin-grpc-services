using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.ApiGateway.Core.Dtos;
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
        const string channelUrlStorage = "http://localhost:5054";
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
                var reply = await client.GetFrontBusinessListAsync(new BusinessFrontQueryFilter { PageId = pageId, PageSize = pageSize });

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
        [HttpGet("GetFrontBusinessListByTagId")]
        public async Task<ActionResult> GetFrontBusinessListByTagId([FromQuery] Guid tagId, [FromQuery] int pageId = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new BusinessAppService.BusinessAppServiceClient(channel);
                var reply = await client.GetFrontBusinessListByTagIdAsync(new BusinessFrontTagIdQueryFilter { TagId = tagId.ToString(), PageId = pageId, PageSize = pageSize });

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
        [HttpGet("UpdateLocation")]
        public async Task<ActionResult> UpdateLocation()
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new BusinessAppService.BusinessAppServiceClient(channel);
                var reply = await client.UpdatePointAsync(new BusinessQueryFilter { PageId = 1, PageSize = 10 });

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
        [HttpGet("GetFrontBusinessByLocation")]
        public async Task<ActionResult> GetFrontBusinessByLocation([FromQuery] string query, [FromQuery] int pageId = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new BusinessAppService.BusinessAppServiceClient(channel);
                var reply = await client.GetFrontBusinessByLocationAsync(new BusinessFrontLocationQueryFilter { Query = query, Longitude = 39.236581, Latitude = 40.962990, PageId = pageId, PageSize = pageSize });

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
        [HttpGet("GetFrontSingleBusiness")]
        public async Task<ActionResult> GetFrontSingleBusiness([FromQuery] Guid businessId)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new BusinessAppService.BusinessAppServiceClient(channel);
                var reply = await client.GetFrontByIdAsync(new BusinessIdFilter { BusinessId = businessId.ToString() });

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
        public async Task<ActionResult> Post([FromForm] CreateBusinessDto entity)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new BusinessAppService.BusinessAppServiceClient(channel);

                var business = new CreateBusinessGm
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
                };

                var tags = from tag in entity.Tags
                           select new CreateBusinessGm.Types.TagGm
                           {
                               TagId = tag.Id.ToString(),
                           };

                business.Tags.AddRange(tags);

                var reply = await client.PostAsync(business, headers: GetHeader());

                var galleryClient = new BusinessGalleryAppService.BusinessGalleryAppServiceClient(channel);


                List<FileModel> fileModels = new List<FileModel>();

                if(entity.Files!=null)
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
                        BusinessId = reply.Id,
                    }, headers: GetHeader());
                }


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
