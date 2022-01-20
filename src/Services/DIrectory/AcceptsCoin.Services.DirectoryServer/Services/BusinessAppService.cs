using System;
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
        private IBusinessTagRepository _businessTagRepository;
        private ICategoryRepository _categoryRepository;
        private ITagRepository  _tagRepository;
        public BusinessGrpcService(ILogger<BusinessGrpcService> logger, IBusinessRepository businessRepository
            , ICategoryRepository categoryRepository, IBusinessTagRepository businessTagRepository, ITagRepository tagRepository)
        {
            _logger = logger;
            _businessRepository = businessRepository;
            _businessTagRepository = businessTagRepository;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
        }

        private Guid getUserId(ServerCallContext context)
        {
            return Guid.Parse(context.GetHttpContext().User.Identity.Name);
        }
        private string getPartnetId(ServerCallContext context)
        {
            return "bff3b2dd-e89d-46fc-a868-aab93a3efbbe";
        }



        [AllowAnonymous]
        public override async Task<SingleBusinessFrontGm> GetFrontById(BusinessIdFilter request, ServerCallContext context)
        {
            var business = await _businessRepository.Find(request.BusinessId);
            SingleBusinessFrontGm response = new SingleBusinessFrontGm();


            response.Id = business.BusinessId.ToString();
            response.Latitude = business.Latitude;
            response.Longitude = business.Longitude;
            response.Icon = "icon";

            response.ImageUrl = business.BusinessGalleries.Count > 0 ?
                business.BusinessGalleries.FirstOrDefault().Name + business.BusinessGalleries.FirstOrDefault().Extension
                :
                "https://img.grouponcdn.com/deal/28PbQPC6SSX8BASL8NRkTSK4Ayoe/28-1200x720/v1/c870x524.webp";

            response.LocationName = "United state";
            response.Rate = 5;
            response.Subtitle = business.Description;
            response.Title = business.Name;
            response.TotalRate = 100;
            response.Category = new BusinessCategoryFrontGm
            {
                Id = business.Category.CategoryId.ToString(),
                Title = "Category Title"
            };

            var images = from image in business.BusinessGalleries
                         select new BusinessGalleryFrontGm()
                         {
                             Id = image.BusinessGalleryId.ToString(),
                             Name = image.Name,
                             Extension = image.Extension,
                         };
            response.Images.AddRange(images.ToArray());


            var tags = from tag in business.BusinessTags
                       select new BusinessTagFrontGm()
                       {
                           Id = tag.TagId.ToString(),
                           Title = "Tag Title"
                       };

            response.Tags.AddRange(tags.ToArray());


            var reviews = from review in business.BusinessReviews
                          select new BusinessReviewFrontGm()
                          {
                              Id = review.ReviewId.ToString(),
                              Comment = review.Message,
                              Name = "Name Family",
                              Rate = review.Rate,
                          };
            response.Reviews.AddRange(reviews.ToArray());



            var tokens = from token in business.BusinessGalleries
                         select new BusinessTokenFrontGm()
                         {
                             Id = token.BusinessId.ToString(),
                             Coin = "Coin Name",
                             Img = "/coin/xrplpay.png",
                             TotalCoin = 100,
                         };
            response.Reviews.AddRange(reviews.ToArray());
            return await Task.FromResult(response);
        }
        [AllowAnonymous]
        public override async Task<BusinessListFrontGm> GetFrontBusinessList(BusinessFrontQueryFilter request, ServerCallContext context)
        {
            BusinessListFrontGm response = new BusinessListFrontGm();

            IQueryable<Business> query = _businessRepository.GetQuery();


            response.CurrentPage = request.PageId;
            response.ItemCount = await _businessRepository.GetCount(query);
            response.PageCount = (response.ItemCount / request.PageSize) + 1;

            var buinessList = await _businessRepository.GetAll(query, request.PageId, request.PageSize);

            var businesses = from business in buinessList
                             select new BusinessFrontGm()
                             {
                                 Id = business.BusinessId.ToString(),
                                 Latitude = business.Latitude,
                                 Longitude = business.Longitude,
                                 Icon = "icon",
                                 //ImageUrl = "https://img.grouponcdn.com/deal/28PbQPC6SSX8BASL8NRkTSK4Ayoe/28-1200x720/v1/c870x524.webp",
                                 ImageUrl = business.BusinessGalleries.Count > 0 ? business.BusinessGalleries.FirstOrDefault().Name : "https://img.grouponcdn.com/deal/28PbQPC6SSX8BASL8NRkTSK4Ayoe/28-1200x720/v1/c870x524.webp",
                                 LocationName = "United state",
                                 Rate = 5,
                                 Subtitle = business.Description,
                                 Title = business.Name,
                                 TotalRate = 100,

                             };

            response.Items.AddRange(businesses.ToArray());

            for (int i = 0; i < businesses.Count() - 1; i++)
            {
                var tokens = from token in buinessList
                             select new BusinessFrontGm.Types.BusinessTokenFrontGm()
                             {
                                 Id = Guid.NewGuid().ToString(),
                                 Coin = "Token",
                                 Img = "/coin/xrplpay.png",
                                 TotalCoin = 10,
                             };
                //businesses.ElementAt(i).Token.Concat(tokens.ToArray());
                response.Items[i].Token.AddRange(tokens.ToArray());
            }


            return await Task.FromResult(response);
        }

        [AllowAnonymous]
        public override async Task<BusinessListFrontGm> GetFrontBusinessListByTagId(BusinessFrontTagIdQueryFilter request, ServerCallContext context)
        {
            BusinessListFrontGm response = new BusinessListFrontGm();

            IQueryable<Business> query = _businessRepository.GetQuery();
            query = query.Where(x => x.BusinessTags.Any(c => c.TagId == Guid.Parse(request.TagId)));

            response.CurrentPage = request.PageId;
            response.ItemCount = await _businessRepository.GetCount(query);
            response.PageCount = (response.ItemCount / request.PageSize) + 1;

            var buinessList = await _businessRepository.GetAll(query, request.PageId, request.PageSize);

            var businesses = from business in buinessList
                             select new BusinessFrontGm()
                             {
                                 Id = business.BusinessId.ToString(),
                                 Latitude = business.Latitude,
                                 Longitude = business.Longitude,
                                 Icon = "icon",
                                 //ImageUrl = "https://img.grouponcdn.com/deal/28PbQPC6SSX8BASL8NRkTSK4Ayoe/28-1200x720/v1/c870x524.webp",
                                 ImageUrl = business.BusinessGalleries.Count > 0 ? business.BusinessGalleries.FirstOrDefault().Name : "https://img.grouponcdn.com/deal/28PbQPC6SSX8BASL8NRkTSK4Ayoe/28-1200x720/v1/c870x524.webp",
                                 LocationName = "United state",
                                 Rate = 5,
                                 Subtitle = business.Description,
                                 Title = business.Name,
                                 TotalRate = 100,

                             };

            response.Items.AddRange(businesses.ToArray());

            for (int i = 0; i < businesses.Count() - 1; i++)
            {
                var tokens = from token in buinessList
                             select new BusinessFrontGm.Types.BusinessTokenFrontGm()
                             {
                                 Id = Guid.NewGuid().ToString(),
                                 Coin = "Token",
                                 Img = "/coin/xrplpay.png",
                                 TotalCoin = 10,
                             };
                //businesses.ElementAt(i).Token.Concat(tokens.ToArray());
                response.Items[i].Token.AddRange(tokens.ToArray());
            }


            return await Task.FromResult(response);
        }
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
                                 Id = business.BusinessId.ToString(),
                                 Name=business.Name,
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
                             };

            response.Items.AddRange(businesses.ToArray());
            response.Pagination = pagination;
            return await Task.FromResult(response);

        }
        public override async Task<BusinessListGm> GetByUserId(BusinessQueryFilter request, ServerCallContext context)
        {
            BusinessListGm response = new BusinessListGm();
            PaginationGm pagination = new PaginationGm();

            IQueryable<Business> query = _businessRepository.GetQuery();

            query = query.Where(x => x.CreatedById == getUserId(context));

            pagination.CurrentPage = request.PageId;
            pagination.ItemCount = await _businessRepository.GetCount(query);
            pagination.PageCount = (pagination.ItemCount / request.PageSize) + 1;



            var businesses = from business in await _businessRepository.GetAll(query, request.PageId, request.PageSize)
                             select new BusinessGm()
                             {
                                 Id = business.BusinessId.ToString(),
                                 Email = business.Email,
                                 Name = business.Name,
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
                             };

            response.Items.AddRange(businesses.ToArray());
            response.Pagination = pagination;
            return await Task.FromResult(response);

        }
        public override async Task<BusinessListGm> GetByPartnerId(BusinessPartnerIdQueryFilter request, ServerCallContext context)
        {
            BusinessListGm response = new BusinessListGm();
            PaginationGm pagination = new PaginationGm();

            IQueryable<Business> query = _businessRepository.GetQuery();

            query = query.Where(x => x.PartnerId == Guid.Parse(request.PartnerId));

            pagination.CurrentPage = request.PageId;
            pagination.ItemCount = await _businessRepository.GetCount(query);
            pagination.PageCount = (pagination.ItemCount / request.PageSize) + 1;



            var businesses = from business in await _businessRepository.GetAll(query, request.PageId, request.PageSize)
                             select new BusinessGm()
                             {
                                 Id = business.BusinessId.ToString(),
                                 Email = business.Email,
                                 WebSiteUrl = business.WebSiteUrl,
                                 ContactNumber = business.ContactNumber,
                                 Logo = business.Logo,
                                 Name = business.Name,
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
                Id = business.BusinessId.ToString(),
                Email = business.Email,
                WebSiteUrl = business.WebSiteUrl,
                ContactNumber = business.ContactNumber,
                Name = business.Name,
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
            };
            return await Task.FromResult(searchedBusiness);
        }

        public override async Task<BusinessGm> Post(CreateBusinessGm request, ServerCallContext context)
        {

            var category = await _categoryRepository.Find(request.CategoryId);

            if (category == null)
            {
                await _categoryRepository.Add(new Category { CategoryId = Guid.Parse(request.CategoryId) });
            }

            var business = new Business()
            {
                BusinessId = Guid.NewGuid(),
                Name = request.Name,
                CreatedById = getUserId(context),
                CategoryId = Guid.Parse(request.CategoryId),
                PartnerId = Guid.Parse(getPartnetId(context)),
                CreatedDate = DateTime.Now,
                Published = true,
                Email = request.Email,
                WebSiteUrl = request.WebSiteUrl,
                ContactNumber = request.ContactNumber,
                Logo = "",
                Owner = request.Owner,
                Manager = request.Manager,
                Twitter = request.Twitter,
                FaceBook = request.FaceBook,
                Instagram = request.Instagram,
                Verified = false,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                Description = request.Description,
                Address = request.Address,
                OfferedServices = request.OfferedServices,
            };

            var res = await _businessRepository.Add(business);


            foreach (var tagGm in request.Tags)
            {

                var tag = await _tagRepository.Find(tagGm.TagId);

                if (tag == null)
                {
                    await _tagRepository.Add(new Tag { TagId = Guid.Parse(tagGm.TagId) });
                }

                BusinessTag item = await _businessTagRepository.Find(res.BusinessId, Guid.Parse(tagGm.TagId));

                if (item == null)
                {
                    var partnerToken = new BusinessTag()
                    {
                        BusinessId = res.BusinessId,
                        TagId = Guid.Parse(tagGm.TagId),
                    };

                    await _businessTagRepository.Add(partnerToken);
                }
                else
                {
                    await _businessTagRepository.Delete(item);
                }
            }

            


            var response = new BusinessGm()
            {
                Id = res.BusinessId.ToString(),

                Email = res.Email,
                WebSiteUrl = res.WebSiteUrl,
                ContactNumber = res.ContactNumber,
                Logo = res.Logo,
                Owner = res.Owner,
                Manager = res.Manager,
                Name = business.Name,
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
            };
            return await Task.FromResult(response);
        }


        public override async Task<BusinessGm> Put(BusinessGm request,
          ServerCallContext context)
        {
            Business business = await _businessRepository.Find(request.Id);
            if (business == null)
            {
                return await Task.FromResult<BusinessGm>(null);
            }


            business.Name = request.Name;
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
            business.UpdatedById = getUserId(context);
            business.UpdatedDate = DateTime.Now;




            await _businessRepository.Update(business);
            return await Task.FromResult<BusinessGm>(new BusinessGm()
            {
                Id = business.BusinessId.ToString(),
                Email = business.Email,
                WebSiteUrl = business.WebSiteUrl,
                ContactNumber = business.ContactNumber,
                Logo = business.Logo,
                Owner = business.Owner,
                Name=business.Name,
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
            business.UpdatedById = getUserId(context);
            business.UpdatedDate = DateTime.Now;

            await _businessRepository.Update(business);
            return await Task.FromResult<Empty>(new Empty());
        }
        public override async Task<Empty> SoftDeleteCollection(BusinessDeleteCollectionGm request, ServerCallContext context)
        {

            foreach (var item in request.Items)
            {
                Business business = await _businessRepository.Find(item.BusinessId);

                if (business == null)
                {
                    return await Task.FromResult<Empty>(null);
                }

                business.Deleted = true;
                business.UpdatedById = getUserId(context);
                business.UpdatedDate = DateTime.Now;

                await _businessRepository.Update(business);
            }

            return await Task.FromResult<Empty>(new Empty());
        }
    }
}
