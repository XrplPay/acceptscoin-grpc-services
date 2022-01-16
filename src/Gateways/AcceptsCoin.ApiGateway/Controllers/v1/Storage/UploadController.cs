using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.ApiGateway.Core.Views;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AcceptsCoin.Services.StorageServer.Protos;
using Google.Protobuf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Grpc.Core;
using Microsoft.Net.Http.Headers;

namespace AcceptsCoin.ApiGateway.Controllers.v1.Storage
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v1/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class UploadController : Controller
    {
        const string channelUrl = "http://localhost:5054";
        private readonly IWebHostEnvironment _environment;
        public UploadController( IWebHostEnvironment environment)
        {
            
            _environment = environment;
        }
        private Metadata GetHeader()
        {
            var accessToken = Request.Headers[HeaderNames.Authorization];

            var header = new Metadata();
            header.Add("Authorization", accessToken);
            return header;
        }
        public class ModelFile
        {
            public List<IFormFile> File { get; set; }

            public Guid BusinessId { get; set; }
        }

        [HttpPost("Uploaded")]
        public async Task<ActionResult> Uploaded()
        {
            try
            {
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new UploadFileAppService.UploadFileAppServiceClient(channel);
                var reply = await client.FileUpLoad();

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

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload([FromForm] ModelFile files)
        {
            var userId = Guid.Parse("999bb90f-3167-4f81-83bb-0c76d1d3ace5");
            //var gallery = new Gallery();
            if (string.IsNullOrWhiteSpace(_environment.WebRootPath))
            {
                string path = System.IO.Directory.GetCurrentDirectory();
                _environment.WebRootPath = Path.Combine(path, "wwwroot");
            }


            var channel = GrpcChannel.ForAddress(channelUrl);
            var client = new UploadFileAppService.UploadFileAppServiceClient(channel);


            foreach (var file in files.File)
            {
                if (file.Length > 0)
                {
                    //StreamWriter sw = null;
                    //StreamReader sr = null;

                    var index = file.FileName.IndexOf(".");
                    var extension = file.FileName.Substring(index, file.FileName.Length - index);
                    var newName = Guid.NewGuid().ToString();

                    //await file.CopyToAsync(fileStream);

                    //await file.CopyToAsync(ms);

                    //sw = new StreamWriter(ms);
                    //sr = new StreamReader(ms);

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

                    while ((content.ReadedByte =await ms.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        content.Buffer = ByteString.CopyFrom(buffer);
                        await upload.RequestStream.WriteAsync(content);
                    }
                    await upload.RequestStream.CompleteAsync();

                    file.OpenReadStream().Close();





                    //GalleryCreateDto galleryCreateDto = new GalleryCreateDto
                    //{
                    //    BusinessId = files.BusinessId,
                    //    Extension = extension,
                    //    Name = newName,
                    //    Published = true,
                    //};

                    //gallery = await _galleryServices.Create(userId, galleryCreateDto);

                }
            }

            //return Ok(gallery);
            return Ok();
        }
    }
}
