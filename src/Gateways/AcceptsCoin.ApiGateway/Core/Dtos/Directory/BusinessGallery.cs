using System;
using Microsoft.AspNetCore.Http;

namespace AcceptsCoin.ApiGateway.Core.Dtos.Directory
{
    public class CreateBusinessGalleryDto
    {
        public Guid  BusinessId{ get; set; }

        public IFormCollection Files { get; set; }

    }
}
