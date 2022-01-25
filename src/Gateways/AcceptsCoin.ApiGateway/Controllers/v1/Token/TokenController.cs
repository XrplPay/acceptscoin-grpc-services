﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.ApiGateway.Core.Dtos;
using AcceptsCoin.ApiGateway.Core.Views;
using AcceptsCoin.Services.CoreServer.Protos;
using AcceptsCoin.Services.DirectoryServer.Protos;
using AcceptsCoin.Services.StorageServer.Protos;
using AcceptsCoin.Services.TokenServer ;
using AcceptsCoin.Services.TokenServer.Protos;
using Google.Protobuf;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Empty = AcceptsCoin.Services.TokenServer.Protos.Empty;

namespace AcceptsCoin.ApiGateway.Controllers.v1.Token
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v1/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TokenController : ControllerBase
    {
        const string coreChannelUrl = "http://localhost:5052";
        const string directoryChannelUrl = "http://localhost:5053";
        const string channelUrl = "http://localhost:5055";
        const string channelUrlStorage = "http://localhost:5054";
        public TokenController()
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
                var client = new TokenAppService.TokenAppServiceClient(channel);
                var reply = await client.GetAllAsync(new TokenQueryFilter { PageId = pageId, PageSize = pageSize }, headers: GetHeader());

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
                var client = new TokenAppService.TokenAppServiceClient(channel);
                var reply = await client.GetByPartnerIdAsync(new Services.TokenServer.Protos.PartnerTokenQueryFilter { PartnerId = partnerId.ToString(), PageId = pageId, PageSize = pageSize }, headers: GetHeader());

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


        [HttpGet("GetByBusinessIdFromDirectoryService")]
        public async Task<ActionResult> GetByBusinessIdFromDirectoryService([FromQuery] Guid businessId, [FromQuery] int pageId = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(directoryChannelUrl);
                var client = new DirectoryAppService.DirectoryAppServiceClient(channel);
                var reply = await client.GetTokenListByBusinessIdAsync(new TokenBusinessIdQueryFilter { BusinessId = businessId.ToString(), PageId = pageId, PageSize = pageSize }, headers: GetHeader());

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


        [HttpPost("SavePartnerToken")]
        public async Task<ActionResult> SavePartnerToken([FromBody] PartnerTokenDto partnerToken)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new TokenAppService.TokenAppServiceClient(channel);
                await client.SavePartnerTokenAsync(new PartnerTokenGm { PartnerId = partnerToken.PartnerId.ToString(), TokenId = partnerToken.TokenId.ToString() }, headers: GetHeader());

                var coreChannel = GrpcChannel.ForAddress(coreChannelUrl);
                var coreClient = new CoreAppService.CoreAppServiceClient(channel);
                await coreClient.SavePartnerTokenAsync(new CorePartnerTokenGm { PartnerId = partnerToken.PartnerId.ToString(), TokenId = partnerToken.TokenId.ToString() }, headers: GetHeader());

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

      


        [AllowAnonymous]
        [HttpGet("GetFrontTokenList")]
        public async Task<ActionResult> GetFrontTokenList()
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new TokenAppService.TokenAppServiceClient(channel);
                var reply = await client.GetFrontTokenListAsync(new Empty());

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
                var client = new TokenAppService.TokenAppServiceClient(channel);
                var reply = await client.GetByIdAsync(new TokenIdFilter { TokenId = id.ToString() }, headers: GetHeader());

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
        public async Task<ActionResult> Post([FromForm] CreateTokenDto entity)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new TokenAppService.TokenAppServiceClient(channel);
                var logo = await Upload(entity.File);
                var reply = await client.PostAsync(new TokenGm
                {
                    Id = "",
                    Name = entity.Name,
                    Symbol = entity.Symbol,
                    Description = entity.Description,
                    Icon = "",
                    Logo = logo,
                    Priority = entity.Priority,
                    Link = entity.Link,
                }, headers: GetHeader());


                #region Dependency
                if (reply != null && reply.Id != null)
                {
                    var directoryChannel = GrpcChannel.ForAddress(directoryChannelUrl);
                    var directoryClient = new DirectoryAppService.DirectoryAppServiceClient(directoryChannel);
                    await directoryClient.DirectoryTokenPostAsync(new DirectoryTokenGm
                    {
                        Id = reply.Id,
                        Name = entity.Name,
                        Icon = "",
                        Logo = logo,
                        Symbol = entity.Symbol,

                    }, headers: GetHeader());
                }
                #endregion


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
        public async Task<ActionResult> Put(Guid id, [FromForm] UpdateTokenDto entity)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new TokenAppService.TokenAppServiceClient(channel);


                string logo;


                if (entity.File != null)
                {
                    logo = await Upload(entity.File);
                }
                else
                {
                    logo = entity.Logo;
                }

                var reply = await client.PutAsync(new UpdateTokenGm
                {
                    Id = id.ToString(),
                    Name = entity.Name,
                    Symbol = entity.Symbol,
                    Description = entity.Description,

                    Icon = "",
                    
                    Logo = logo,
                    Priority = entity.Priority,
                    Link = entity.Link,
                }, headers: GetHeader());


                


                #region Dependency
                if (reply != null && reply.Id != null)
                {
                    var directoryChannel = GrpcChannel.ForAddress(directoryChannelUrl);
                    var directoryClient = new DirectoryAppService.DirectoryAppServiceClient(directoryChannel);
                    directoryClient.DirectoryTokenPut(new DirectoryTokenGm
                    {
                        Id = id.ToString(),
                        Name = entity.Name,
                        Icon = "",
                        Logo = logo,
                        Symbol = entity.Symbol,

                    }, headers: GetHeader());
                }
                #endregion


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
                var client = new TokenAppService.TokenAppServiceClient(channel);
                var reply = await client.SoftDeleteAsync(new TokenIdFilter { TokenId = id.ToString() }, headers: GetHeader());

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
                var client = new TokenAppService.TokenAppServiceClient(channel);

                DeleteCollectionGm collectionGm = new DeleteCollectionGm();
                foreach (var item in items.Items)
                {
                    collectionGm.Items.Add(new TokenIdFilter { TokenId = item.Id.ToString() });
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
