using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.Services.IdentityServer.Domain.Interfaces;
using AcceptsCoin.Services.IdentityServer.Domain.Models;
using AcceptsCoin.Services.IdentityServer.Protos;
using Grpc.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace AcceptsCoin.Services.IdentityServer.Services
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RoleGrpcService : RoleAppService.RoleAppServiceBase
    {
        private readonly ILogger<RoleGrpcService> _logger;
        private IRoleRepository _roleRepository;
        private IPartnerRepository _partnerRepository;
        public RoleGrpcService(ILogger<RoleGrpcService> logger, IRoleRepository roleRepository, IPartnerRepository partnerRepository)
        {
            _logger = logger;
            _roleRepository = roleRepository;
            _partnerRepository = partnerRepository;
        }

        private Guid getRoleId(ServerCallContext context)
        {
            return Guid.Parse(context.GetHttpContext().User.Identity.Name);
        }
        private string getPartnetId(ServerCallContext context)
        {
            return "bff3b2dd-e89d-46fc-a868-aab93a3efbbe";
        }

        public override async Task<RoleListGm> GetAll(RoleQueryFilter request, ServerCallContext context)
        {
            RoleListGm response = new RoleListGm();

            IQueryable<Role> query = _roleRepository.GetQuery();


            response.CurrentPage = request.PageId;
            response.ItemCount = await _roleRepository.GetCount(query);
            response.PageCount = (response.ItemCount / request.PageSize) + 1;


            var roles = from role in await _roleRepository.GetAll(query, request.PageId, request.PageSize)
                        select new RoleGm()
                        {
                            Id = role.RoleId.ToString(),
                            Name = role.Name,

                        };
            response.Items.AddRange(roles.ToArray());
            return await Task.FromResult(response);
        }


        public override async Task<RoleListGm> GetByUserId(UserRoleQueryFilter request, ServerCallContext context)
        {
            RoleListGm response = new RoleListGm();

            //PaginationGm pagination = new PaginationGm();

            IQueryable<Role> query = _roleRepository.GetQuery();
            query = query.Where(x => x.UserRoles.Any(c => c.UserId == Guid.Parse(request.UserId)));



            var roles = from role in await _roleRepository.GetAll(query, request.PageId, request.PageSize)
                        select new RoleGm()
                        {
                            Id = role.RoleId.ToString(),
                            Name = role.Name,
                        };
            response.Items.AddRange(roles.ToArray());
            //response.Pagination = pagination;
            return await Task.FromResult(response);
        }

        public override async Task<RoleGm> GetById(RoleIdFilter request, ServerCallContext context)
        {
            var role = await _roleRepository.Find(Guid.Parse(request.RoleId));
            var searchedRole = new RoleGm()
            {
                Id = role.RoleId.ToString(),
                Name = role.Name,
            };
            return await Task.FromResult(searchedRole);
        }
     



        public override async Task<RoleGm> Post(RoleGm request, ServerCallContext context)
        {
            var role = new Role()
            {
                RoleId = Guid.NewGuid(),
                Name = request.Name,
                CreatedById = getRoleId(context),
                CreatedDate = DateTime.Now,
                Published = true,
            };

            var res = await _roleRepository.Add(role);

            var response = new RoleGm()
            {
                Id = res.RoleId.ToString(),

                Name = res.Name,
            };
            return await Task.FromResult(response);
        }


        public override async Task<RoleGm> Put(RoleGm request,
          ServerCallContext context)
        {
            Role role = await _roleRepository.Find(request.Id);
            if (role == null)
            {
                return await Task.FromResult<RoleGm>(null);
            }


            role.Name = request.Name;
            role.UpdatedById = getRoleId(context);
            role.UpdatedDate = DateTime.Now;




            await _roleRepository.Update(role);
            return await Task.FromResult<RoleGm>(new RoleGm()
            {
                Id = role.RoleId.ToString(),
                Name = role.Name,
            });
        }
        public override async Task<EmptyRole> Delete(RoleIdFilter request, ServerCallContext context)
        {
            Role role = await _roleRepository.Find(request.RoleId);
            if (role == null)
            {
                return await Task.FromResult<EmptyRole>(null);
            }

            await _roleRepository.Delete(role);
            return await Task.FromResult<EmptyRole>(new EmptyRole());
        }

        public override async Task<EmptyRole> SoftDelete(RoleIdFilter request, ServerCallContext context)
        {
            Role role = await _roleRepository.Find(request.RoleId);

            if (role == null)
            {
                return await Task.FromResult<EmptyRole>(null);
            }

            role.Deleted = true;
            role.UpdatedById = getRoleId(context);
            role.UpdatedDate = DateTime.Now;

            await _roleRepository.Update(role);
            return await Task.FromResult<EmptyRole>(new EmptyRole());
        }
        public override async Task<EmptyRole> SoftDeleteCollection(RoleDeleteCollectionGm request, ServerCallContext context)
        {

            foreach (var item in request.Items)
            {
                Role role = await _roleRepository.Find(item.RoleId);

                if (role == null)
                {
                    return await Task.FromResult<EmptyRole>(null);
                }

                role.Deleted = true;
                role.UpdatedById = getRoleId(context);
                role.UpdatedDate = DateTime.Now;

                await _roleRepository.Update(role);
            }

            return await Task.FromResult<EmptyRole>(new EmptyRole());
        }
    }
}
