﻿using System;
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
    public class PartnerController : ControllerBase
    {
        const string channelUrlStorage = "http://localhost:5054";
        const string channelUrl = "http://localhost:5052";

        public PartnerController()
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
                var client = new PartnerAppService.PartnerAppServiceClient(channel);
                var reply = await client.GetAllAsync(new PartnerQueryFilter { PageId = pageId, PageSize = pageSize }, headers: GetHeader());

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
                var client = new PartnerAppService.PartnerAppServiceClient(channel);
                var reply = await client.GetByIdAsync(new PartnerIdFilter { PartnerId = id.ToString() }, headers: GetHeader());

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
        public async Task<ActionResult> Post([FromForm] CreatePartnerDto entity)
        {
            try

            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new PartnerAppService.PartnerAppServiceClient(channel);
                var logo = await Upload(entity.File);
                var reply = await client.PostAsync(new PartnerGm
                {
                    Id = "",
                    Name = entity.Name,
                    ContactNumber = entity.ContactNumber,
                    Email = entity.Email,
                    LanguageId = entity.LanguageId.ToString(),
                    Logo = logo,
                    Manager = entity.Manager,
                    Owner = entity.Owner,
                    WebSiteUrl = entity.WebSiteUrl,
                    ApiKey = "",
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
        public async Task<ActionResult> Put(Guid id, [FromBody] UpdatePartnerDto entity)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new PartnerAppService.PartnerAppServiceClient(channel);
                var reply = await client.PutAsync(new PartnerGm
                {
                    Id = id.ToString(),
                    Name = entity.Name,
                    ContactNumber = entity.ContactNumber,
                    Email = entity.Email,
                    LanguageId = entity.LanguageId.ToString(),
                    Logo = entity.Logo,
                    Manager = entity.Manager,
                    Owner = entity.Owner,
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
                var client = new PartnerAppService.PartnerAppServiceClient(channel);
                var reply = await client.SoftDeleteAsync(new PartnerIdFilter { PartnerId = id.ToString() }, headers: GetHeader());

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

        [HttpGet("GetByTokenId")]
        public async Task<ActionResult> GetByTokenId([FromQuery] Guid  tokenId, [FromQuery] int pageId = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new PartnerAppService.PartnerAppServiceClient(channel);
                var reply = await client.GetByTokenIdAsync(new PartnerTokenQueryFilter { TokenId = tokenId.ToString(), PageId = pageId, PageSize = pageSize }, headers: GetHeader());

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
                var client = new PartnerAppService.PartnerAppServiceClient(channel);

                PartnerDeleteCollectionGm collectionGm = new PartnerDeleteCollectionGm();
                foreach (var item in items.Items)
                {
                    collectionGm.Items.Add(new PartnerIdFilter { PartnerId = item.Id.ToString() });
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
