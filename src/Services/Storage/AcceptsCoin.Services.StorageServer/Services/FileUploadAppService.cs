using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using AcceptsCoin.Services.StorageServer.Protos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Google.Protobuf.WellKnownTypes;
using System.IO;
using Google.Protobuf;

namespace AcceptsCoin.Services.StorageServer.Services
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FileUploadGrpcService : UploadFileAppService.UploadFileAppServiceBase
    {
        readonly IWebHostEnvironment _webHostEnvironment;

        private readonly ILogger<FileUploadGrpcService> _logger;
        public FileUploadGrpcService(IWebHostEnvironment webHostEnvironment,ILogger<FileUploadGrpcService> logger)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }


       


        public override async Task<Empty> FileUpLoad(IAsyncStreamReader<BytesContent> requestStream, ServerCallContext context)
        {
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "files");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            FileStream fileStream = null;

            try
            {
                int count = 0;

                decimal chunkSize = 0;

                while (await requestStream.MoveNext())
                {
                    if (count++ == 0)
                    {
                        fileStream = new FileStream($"{path}/{requestStream.Current.Info.FileName}{requestStream.Current.Info.FileExtension}", FileMode.CreateNew);

                        fileStream.SetLength(requestStream.Current.FileSize);
                    }

                    var buffer = requestStream.Current.Buffer.ToByteArray();

                    await fileStream.WriteAsync(buffer, 0, requestStream.Current.ReadedByte);

                    Console.WriteLine($"{Math.Round(((chunkSize += requestStream.Current.ReadedByte) * 100) / requestStream.Current.FileSize)}%");
                }
                Console.WriteLine("Yüklendi...");

            }
            catch (Exception ex)
            {
            }
            await fileStream.DisposeAsync();
            fileStream.Close();
            return new Empty();
        }

        
        private Guid getUserId(ServerCallContext context)
        {
            return Guid.Parse(context.GetHttpContext().User.Identity.Name);
        }
        private string getPartnetId(ServerCallContext context)
        {
            return "bff3b2dd-e89d-46fc-a868-aab93a3efbbe";
        }

    }
}
