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
    public class UserGrpcService : UserAppService.UserAppServiceBase
    {
        private readonly ILogger<UserGrpcService> _logger;
        private IUserRepository _userRepository;
        private IPartnerRepository _partnerRepository;
        private IUserRoleRepository _userRoleRepository;
        public UserGrpcService(ILogger<UserGrpcService> logger, IUserRepository userRepository, IPartnerRepository partnerRepository, IUserRoleRepository userRoleRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _partnerRepository = partnerRepository;
            _userRoleRepository = userRoleRepository;
        }

        private Guid getUserId(ServerCallContext context)
        {
            return Guid.Parse(context.GetHttpContext().User.Identity.Name);
        }
        private string getPartnetId(ServerCallContext context)
        {
            return "bff3b2dd-e89d-46fc-a868-aab93a3efbbe";
        }

        public override async Task<UserListGm> GetAll(UserQueryFilter request, ServerCallContext context)
        {
            UserListGm response = new UserListGm();

            IQueryable<User> query = _userRepository.GetQuery();


            response.CurrentPage = request.PageId;
            response.ItemCount = await _userRepository.GetCount(query);
            response.PageCount = (response.ItemCount / request.PageSize) + 1;


            var users = from user in await _userRepository.GetAll(query, request.PageId, request.PageSize)
                        select new UserGm()
                        {
                            Id = user.UserId.ToString(),
                            Name = user.Name,
                            Email = user.Email,
                            Activate = user.Activated,
                            Partner = "Partner",
                            Published = user.Published,
                            UserName = user.UserName,

                        };
            response.Items.AddRange(users.ToArray());
            return await Task.FromResult(response);
        }
        public override async Task<UserListGm> GetByPartnerId(UserPartnerIdQueryFilter request, ServerCallContext context)
        {

            var partner =await _partnerRepository.Find(request.PartnerId);

            if (partner == null)
            {
                await _partnerRepository.Add(new Partner { PartnerId = Guid.Parse(request.PartnerId) });
            }

            UserListGm response = new UserListGm();

            IQueryable<User> query = _userRepository.GetQuery();
            query = query.Where(x => x.PartnerId == Guid.Parse(request.PartnerId));

            response.CurrentPage = request.PageId;
            response.ItemCount = await _userRepository.GetCount(query);
            response.PageCount = (response.ItemCount / request.PageSize) + 1;


            var users = from user in await _userRepository.GetAll(query, request.PageId, request.PageSize)
                        select new UserGm()
                        {
                            Id = user.UserId.ToString(),
                            Name = user.Name,
                            Email = user.Email,
                            Activate = user.Activated,
                            Partner = "Partner",
                            Published = user.Published,
                            UserName = user.UserName,

                        };
            response.Items.AddRange(users.ToArray());
            return await Task.FromResult(response);
        }
        public override async Task<UserGm> GetById(UserIdFilter request, ServerCallContext context)
        {
            var user = await _userRepository.Find(Guid.Parse(request.UserId));
            var searchedUser = new UserGm()
            {
                Id = user.UserId.ToString(),
                Name = user.Name,
                Email = user.Email,
                Activate = user.Activated,
                Partner = "Partner",
                Published = user.Published,
                UserName = user.UserName,
            };
            return await Task.FromResult(searchedUser);
        }
        public override async Task<UserProfileGm> GetProfile(EmptyUser request, ServerCallContext context)
        {
            var user = await _userRepository.Find(getUserId(context));
            var userProfile = new UserProfileGm()
            {
                Name = user.Name,
                Email = user.Email,
            };
            return await Task.FromResult(userProfile);
        }

        public override async Task<UserGm> Post(UserGm request, ServerCallContext context)
        {
            var user = new User()
            {
                UserId = Guid.NewGuid(),
                Name = request.Name,
                CreatedById = getUserId(context),
                CreatedDate = DateTime.Now,
                Published = true,
                Email = request.Email,
                UserName=request.Email,
                Password=request.Password,
                Activated=true,
            };

            var res = await _userRepository.Add(user);

            var response = new UserGm()
            {
                Id = res.UserId.ToString(),

                Email = res.Email,
                Name = res.Name,

                Activate = res.Activated,
                Partner = "Partner",
                Published = res.Published,
                UserName = res.UserName,
            };
            return await Task.FromResult(response);
        }


        public override async Task<UserGm> Put(UserGm request,
          ServerCallContext context)
        {
            User user = await _userRepository.Find(request.Id);
            if (user == null)
            {
                return await Task.FromResult<UserGm>(null);
            }


            user.Email = request.Email;
            user.Name = request.Name;
            user.UpdatedById = getUserId(context);
            user.UpdatedDate = DateTime.Now;




            await _userRepository.Update(user);
            return await Task.FromResult<UserGm>(new UserGm()
            {
                Id = user.UserId.ToString(),
                Email = user.Email,
                Name = user.Name,
                Activate = user.Activated,
                Partner = "Partner",
                Published = user.Published,
                UserName = user.UserName,
            });
        }
        public override async Task<EmptyUser> Delete(UserIdFilter request, ServerCallContext context)
        {
            User user = await _userRepository.Find(request.UserId);
            if (user == null)
            {
                return await Task.FromResult<EmptyUser>(null);
            }

            await _userRepository.Delete(user);
            return await Task.FromResult<EmptyUser>(new EmptyUser());
        }

        public override async Task<EmptyUser> SoftDelete(UserIdFilter request, ServerCallContext context)
        {
            User user = await _userRepository.Find(request.UserId);

            if (user == null)
            {
                return await Task.FromResult<EmptyUser>(null);
            }

            user.Deleted = true;
            user.UpdatedById = getUserId(context);
            user.UpdatedDate = DateTime.Now;

            await _userRepository.Update(user);
            return await Task.FromResult<EmptyUser>(new EmptyUser());
        }
        public override async Task<EmptyUser> SoftDeleteCollection(UserDeleteCollectionGm request, ServerCallContext context)
        {

            foreach (var item in request.Items)
            {
                User user = await _userRepository.Find(item.UserId);

                if (user == null)
                {
                    return await Task.FromResult<EmptyUser>(null);
                }

                user.Deleted = true;
                user.UpdatedById = getUserId(context);
                user.UpdatedDate = DateTime.Now;

                await _userRepository.Update(user);
            }

            return await Task.FromResult<EmptyUser>(new EmptyUser());
        }

        public override async Task<EmptyUser> SaveUserRole(UserRoleGm request, ServerCallContext context)
        {
            UserRole item = await _userRoleRepository.Find(Guid.Parse(request.UserId), Guid.Parse(request.RoleId));

            if (item == null)
            {
                var userRole = new UserRole()
                {
                    RoleId = Guid.Parse(request.RoleId),
                    UserId = Guid.Parse(request.UserId),
                };

                await _userRoleRepository.Add(userRole);
            }
            else
            {
                await _userRoleRepository.Delete(item);
            }

            return await Task.FromResult<EmptyUser>(new EmptyUser());
        }
    }
}
