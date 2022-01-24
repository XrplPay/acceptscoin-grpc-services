using System;
using System.Net;
using System.Threading.Tasks;
using AcceptsCoin.Services.CoreServer.Domain.Interfaces;
using AcceptsCoin.Services.CoreServer.Domain.Models;
using AcceptsCoin.Services.CoreServer.Protos;
using Grpc.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace AcceptsCoin.Services.CoreServer.Services
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class CoreGrpcService : CoreAppService.CoreAppServiceBase
    {
        private IPartnerTokenRepository _partnerTokenRepository;
        private readonly ILogger<CoreGrpcService> _logger;
        public CoreGrpcService(ILogger<CoreGrpcService> logger, IPartnerTokenRepository partnerTokenRepository)
        {
            _logger = logger;
            _partnerTokenRepository = partnerTokenRepository;

        }
        private Guid getUserId(ServerCallContext context)
        {
            return Guid.Parse(context.GetHttpContext().User.Identity.Name);
        }
        private Guid getPartnerId(ServerCallContext context)
        {
            return Guid.Parse("bff3b2dd-e89d-46fc-a868-aab93a3efbbe");
        }
        public override async Task<CorePartnerTokenGm> SavePartnerToken(CorePartnerTokenGm request, ServerCallContext context)
        {

            var item = await _partnerTokenRepository.Find(Guid.Parse(request.TokenId), Guid.Parse(request.PartnerId));

            if (item == null)
            {
                var partnerToken = new PartnerToken()
                {
                    TokenId = Guid.Parse(request.TokenId),
                    CreatedById = getUserId(context),
                    CreatedDate = DateTime.Now,
                    Deleted = false,
                    PartnerId = getPartnerId(context),
                    Published = true,
                };

                await _partnerTokenRepository.Add(partnerToken);

            }
            else
            {
                await _partnerTokenRepository.Delete(item);
            }

            var response = new CorePartnerTokenGm()
            {
                PartnerId = request.PartnerId,
                TokenId = request.TokenId,
            };
            return await Task.FromResult(response);
        }

    }
}
