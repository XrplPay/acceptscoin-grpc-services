﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.Services.DirectoryServer.Domain.Interfaces;
using AcceptsCoin.Services.DirectoryServer.Domain.Models;
using AcceptsCoin.Services.DirectoryServer.Protos;
using Grpc.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace AcceptsCoin.Services.DirectoryServer
{

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BusinessGrpcService : BusinessAppService.BusinessAppServiceBase
    {
        private readonly ILogger<BusinessGrpcService> _logger;
        private IBusinessRepository _businessRepository;
        public BusinessGrpcService(ILogger<BusinessGrpcService> logger, IBusinessRepository businessRepository)
        {
            _logger = logger;
            _businessRepository = businessRepository;
        }

        private string getUserId(ServerCallContext context)
        {
            return context.GetHttpContext().User.Identity.Name;
        }
        private string getPartnetId(ServerCallContext context)
        {
            return "bff3b2dd-e89d-46fc-a868-aab93a3efbbe";
        }

        [AllowAnonymous]
        public override async Task<BusinessListGm> GetAll(BusinessQueryFilter request, ServerCallContext context)
        {
            BusinessListGm response = new BusinessListGm();
            PaginationGm pagination = new PaginationGm();

            IQueryable<Business> query = _businessRepository.GetQuery();

            
            pagination.CurrentPage = request.PageId;
            pagination.ItemCount = await _businessRepository.GetCount(query);
            pagination.PageCount = (pagination.ItemCount / request.PageSize) + 1;



            var businesses = from business in await _businessRepository.GetAll(query, request.PageId, request.PageSize)
                             select new BusinessGm()
                             {
                                 BusinessId = business.BusinessId.ToString(),
                                 Email = business.Email,
                                 WebSiteUrl = business.WebSiteUrl,
                                 ContactNumber = business.ContactNumber,
                                 Logo = business.Logo,
                                 Owner = business.Owner,
                                 Manager = business.Manager,
                                 Twitter = business.Twitter,
                                 FaceBook = business.FaceBook,
                                 Instagram = business.Instagram,
                                 Verified = business.Verified,
                                 Latitude = business.Latitude,
                                 Longitude = business.Longitude,
                                 Description = business.Description,
                                 Address = business.Address,
                                 OfferedServices = business.OfferedServices,
                                 CategoryId = business.CategoryId.ToString(),
                                 PartnerId = business.PartnerId.ToString(),


                             };

            response.Items.AddRange(businesses.ToArray());
            response.Pagination = pagination;
            return await Task.FromResult(response);

        }
        public override async Task<BusinessGm> GetById(BusinessIdFilter request, ServerCallContext context)
        {
            var business = await _businessRepository.Find(request.BusinessId);
            var searchedBusiness = new BusinessGm()
            {
                BusinessId = business.BusinessId.ToString(),
                Email = business.Email,
                WebSiteUrl = business.WebSiteUrl,
                ContactNumber = business.ContactNumber,
                Logo = business.Logo,
                Owner = business.Owner,
                Manager = business.Manager,
                Twitter = business.Twitter,
                FaceBook = business.FaceBook,
                Instagram = business.Instagram,
                Verified = business.Verified,
                Latitude = business.Latitude,
                Longitude = business.Longitude,
                Description = business.Description,
                Address = business.Address,
                OfferedServices = business.OfferedServices,
                CategoryId = business.CategoryId.ToString(),
                PartnerId = business.PartnerId.ToString(),
            };
            return await Task.FromResult(searchedBusiness);
        }

        public override async Task<BusinessGm> Post(BusinessGm request, ServerCallContext context)
        {
            var business = new Business()
            {
                BusinessId = Guid.NewGuid(),
                Name = request.Name,
                CreatedById = Guid.Parse(getUserId(context)),
                CategoryId = Guid.Parse(request.CategoryId),
                PartnerId = Guid.Parse(getPartnetId(context)),
                CreatedDate = DateTime.Now,
                Published = true,
                Email = request.Email,
                WebSiteUrl = request.WebSiteUrl,
                ContactNumber = request.ContactNumber,
                Logo = request.Logo,
                Owner = request.Owner,
                Manager = request.Manager,
                Twitter = request.Twitter,
                FaceBook = request.FaceBook,
                Instagram = request.Instagram,
                Verified = request.Verified,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                Description = request.Description,
                Address = request.Address,
                OfferedServices = request.OfferedServices,
            };

            var res = await _businessRepository.Add(business);

            var response = new BusinessGm()
            {
                BusinessId = res.BusinessId.ToString(),

                Email = res.Email,
                WebSiteUrl = res.WebSiteUrl,
                ContactNumber = res.ContactNumber,
                Logo = res.Logo,
                Owner = res.Owner,
                Manager = res.Manager,
                Twitter = res.Twitter,
                FaceBook = res.FaceBook,
                Instagram = res.Instagram,
                Verified = res.Verified,
                Latitude = res.Latitude,
                Longitude = res.Longitude,
                Description = res.Description,
                Address = res.Address,
                OfferedServices = res.OfferedServices,
                CategoryId = res.CategoryId.ToString(),
                PartnerId = res.PartnerId.ToString(),
            };
            return await Task.FromResult(response);
        }


        public override async Task<BusinessGm> Put(BusinessGm request,
          ServerCallContext context)
        {
            Business business = await _businessRepository.Find(request.BusinessId);
            if (business == null)
            {
                return await Task.FromResult<BusinessGm>(null);
            }


            business.Email = request.Email;
            business.WebSiteUrl = request.WebSiteUrl;
            business.ContactNumber = request.ContactNumber;
            business.Logo = request.Logo;
            business.Owner = request.Owner;
            business.Manager = request.Manager;
            business.Twitter = request.Twitter;
            business.FaceBook = request.FaceBook;
            business.Instagram = request.Instagram;
            business.Verified = request.Verified;
            business.Latitude = request.Latitude;
            business.Longitude = request.Longitude;
            business.Description = request.Description;
            business.Address = request.Address;
            business.OfferedServices = request.OfferedServices;
            business.CategoryId = Guid.Parse(request.CategoryId);
            business.UpdatedById = Guid.Parse(getUserId(context));
            business.UpdatedDate = DateTime.Now;




            await _businessRepository.Update(business);
            return await Task.FromResult<BusinessGm>(new BusinessGm()
            {
                BusinessId = business.BusinessId.ToString(),
                Email = business.Email,
                WebSiteUrl = business.WebSiteUrl,
                ContactNumber = business.ContactNumber,
                Logo = business.Logo,
                Owner = business.Owner,
                Manager = business.Manager,
                Twitter = business.Twitter,
                FaceBook = business.FaceBook,
                Instagram = business.Instagram,
                Verified = business.Verified,
                Latitude = business.Latitude,
                Longitude = business.Longitude,
                Description = business.Description,
                Address = business.Address,
                OfferedServices = business.OfferedServices,
                CategoryId = business.CategoryId.ToString(),
                PartnerId = business.PartnerId.ToString(),
            });
        }
        public override async Task<Empty> Delete(BusinessIdFilter request, ServerCallContext context)
        {
            Business business = await _businessRepository.Find(request.BusinessId);
            if (business == null)
            {
                return await Task.FromResult<Empty>(null);
            }

            await _businessRepository.Delete(business);
            return await Task.FromResult<Empty>(new Empty());
        }

        public override async Task<Empty> SoftDelete(BusinessIdFilter request, ServerCallContext context)
        {
            Business business = await _businessRepository.Find(request.BusinessId);
            
            if (business == null)
            {
                return await Task.FromResult<Empty>(null);
            }

            business.Deleted = true;
            business.UpdatedById = Guid.Parse(getUserId(context));
            business.UpdatedDate = DateTime.Now;

            await _businessRepository.Update(business);
            return await Task.FromResult<Empty>(new Empty());
        }
    }
}
