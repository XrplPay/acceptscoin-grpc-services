using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AcceptsCoin.Services.CoreServer.Protos;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace AcceptsCoin.ApiGateway.Helper
{

    public interface IDataControl
    {
        public Task<Guid?> getPartnerId(HttpContext context);
        public Task<Guid> getUserId(HttpContext context);
    }

    
    public class DataControl : IDataControl
    {
        //IPartnerServices _partnerServices;
        const string channelUrl = "http://localhost:5052";

        public DataControl()
        {
        }

        private Metadata GetHeader(HttpContext context)
        {
            var accessToken = context.Request.Headers[HeaderNames.Authorization];

            var header = new Metadata();
            header.Add("Authorization", accessToken);
            return header;
        }
        public async Task<Guid?> getPartnerId(HttpContext context)
        {
            StringValues idValues;
            if (context.Request.Headers.TryGetValue("API_KEY", out idValues))
            {
                var apiKey = idValues.First();
                var channel = GrpcChannel.ForAddress(channelUrl);
                var client = new PartnerAppService.PartnerAppServiceClient(channel);
                var reply = await client.GetPartnerIdByApiKeyAsync(new PartnerApiKeyGm { ApiKey = apiKey });

                if(reply.PartnerId!="")
                {
                    return Guid.Parse(reply.PartnerId);
                }
                else
                {
                    return null;
                }
                
            }
            return null;
        }
        public async Task<Guid> getUserId(HttpContext context)
        {
            var userId = Guid.Parse(context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            return userId;
        }
    }
}
