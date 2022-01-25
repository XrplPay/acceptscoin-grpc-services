using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AcceptsCoin.Services.DirectoryServer.Domain.Interfaces;
using AcceptsCoin.Services.DirectoryServer.Domain.Models;
using AcceptsCoin.Services.DirectoryServer.Protos;
using Grpc.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using NetTopologySuite.Operation.Valid;

namespace AcceptsCoin.Services.DirectoryServer
{

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BusinessGrpcService : BusinessAppService.BusinessAppServiceBase
    {
        private readonly ILogger<BusinessGrpcService> _logger;
        private IBusinessRepository _businessRepository;
        private IBusinessTagRepository _businessTagRepository;
        private IBusinessTokenRepository _businessTokenRepository;
        private ICategoryRepository _categoryRepository;
        private ITokenRepository _tokenRepository;
        private ITagRepository  _tagRepository;
        public BusinessGrpcService(ILogger<BusinessGrpcService> logger, IBusinessRepository businessRepository
            , ICategoryRepository categoryRepository, IBusinessTagRepository businessTagRepository, ITagRepository tagRepository, ITokenRepository tokenRepository, IBusinessTokenRepository businessTokenRepository)
        {
            _logger = logger;
            _businessRepository = businessRepository;
            _businessTagRepository = businessTagRepository;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
            _tokenRepository = tokenRepository;
            _businessTokenRepository = businessTokenRepository;
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

            response.ImageUrl = getDefaultImage(business.BusinessGalleries);
            response.WebSiteUrl = business.WebSiteUrl;
            response.LocationName = "United state";
            response.Rate = 5;
            response.Description = business.Description;
            response.Name = business.Name;
            response.Instagram = business.Instagram;
            response.Twitter = business.Twitter;
            response.Facebook = business.FaceBook;
            response.TotalRate = 100;
            response.Email = business.Email;
            response.Manager = business.Manager;
            response.ContactNumbrt = business.ContactNumber;
            response.Owner = business.Owner;
            response.Verified = business.Verified;
            response.OfferedServices = business.OfferedServices;
            response.Category = new BusinessCategoryFrontGm
            {
                Id = business.Category.CategoryId.ToString(),
                Title = "Category Title"
            };

            var images = from image in business.BusinessGalleries.Where(x => x.Deleted == false)
                         select new BusinessGalleryFrontGm()
                         {
                             Id = image.BusinessGalleryId.ToString(),
                             Name = image.Name,
                             Extension = image.Extension,
                             Image = image.Name + image.Extension,
                             Caption = "",
                             //Url = "https://pambansangbakal.com.ph/wp-content/uploads/2019/10/no-image-800x511.png",
                         };
            response.Images.AddRange(images.ToArray());


            var tags = from tag in business.BusinessTags
                       select new BusinessTagFrontGm()
                       {
                           Id = tag.TagId.ToString(),
                           Title = "Tag Title"
                       };

            response.Tags.AddRange(tags.ToArray());


            var reviews = from review in business.BusinessReviews.Where(x => x.Published == true && x.Deleted == false)
                          select new BusinessReviewFrontGm()
                          {
                              Id = review.ReviewId.ToString(),
                              Comment = review.Message,
                              Rate = review.Rate,
                              Date = review.CreatedDate.ToString(),
                              User = new BusinessReviewFrontGm.Types.User
                              {
                                  Email = review.CreatedBy.Email,
                                  Id = review.CreatedBy.UserId.ToString(),
                                  Name = review.CreatedBy.Name,
                                  RateCount = 10,
                                  ReviewCount = 29,
                              },
                          };
            response.Reviews.AddRange(reviews.ToArray());



            var tokens = from token in business.BusinessTokens
                         select new BusinessTokenFrontGm()
                         {
                             Id = token.TokenId.ToString(),
                             Coin = token.Token.Name,
                             Img = token.Token.Logo,
                             TotalCoin = 100,
                         };
            response.Tokens.AddRange(tokens.ToArray());
            return await Task.FromResult(response);
        }//hjhjj
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
                                 //ImageUrl = "https://pambansangbakal.com.ph/wp-content/uploads/2019/10/no-image-800x511.png",
                                 ImageUrl = getDefaultImage(business.BusinessGalleries),
                                 LocationName = "United state",
                                 Rate = 5,
                                 Description = business.Description,
                                 Name = business.Name,
                                 TotalRate = 100,

                             };

            response.Items.AddRange(businesses.ToArray());





            for (int i = 0; i < businesses.Count() - 1; i++)
            {
                var tokens = from token in buinessList.ElementAt(i).BusinessTokens
                             select new BusinessFrontGm.Types.BusinessTokenFrontGm()
                             {
                                 Id = token.TokenId.ToString(),
                                 Coin = token.Token.Name,
                                 Img = token.Token.Logo,
                                 TotalCoin = 10,
                             };
                //businesses.ElementAt(i).Token.Concat(tokens.ToArray());
                response.Items[i].Token.AddRange(tokens.ToArray());
            }


            return await Task.FromResult(response);
        }

        [AllowAnonymous]
        public override async Task<BusinessTagListGm> GetBusinessTag(BusinessIdFilter request, ServerCallContext context)
        {
            BusinessTagListGm response = new BusinessTagListGm();

            IQueryable<BusinessTag> query = _businessTagRepository.GetQuery();
            query = query.Where(x => x.BusinessId == Guid.Parse(request.BusinessId));


            var tagList = await _businessTagRepository.GetAll(query, 1, 1000);

            var tags = from tag in tagList
                       select new BusinessTagGm()
                       {
                           BusinessId = request.BusinessId,
                           TagId = tag.TagId.ToString(),
                       };

            response.BusinessTags.AddRange(tags.ToArray());


            return await Task.FromResult(response);
        }

        [AllowAnonymous]
        public override async Task<BusinessTokenListGm> GetBusinessToken(BusinessIdFilter request, ServerCallContext context)
        {
            BusinessTokenListGm response = new BusinessTokenListGm();

            IQueryable<BusinessToken> query = _businessTokenRepository.GetQuery();
            query = query.Where(x => x.BusinessId == Guid.Parse(request.BusinessId));


            var tagList = await _businessTokenRepository.GetAll(query, 1, 1000);

            var tokens = from token in tagList
                       select new BusinessTokenGm()
                       {
                           BusinessId = request.BusinessId,
                           TokenId = token.TokenId.ToString(),
                       };

            response.BusinessTokens.AddRange(tokens.ToArray());


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
                                 //ImageUrl = "https://pambansangbakal.com.ph/wp-content/uploads/2019/10/no-image-800x511.png",
                                 ImageUrl  = getDefaultImage(business.BusinessGalleries),
                                 LocationName = "United state",
                                 Rate = 5,
                                 Description = business.Description,
                                 Name = business.Name,
                                 TotalRate = 100,

                             };

            response.Items.AddRange(businesses.ToArray());

            for (int i = 0; i < businesses.Count() - 1; i++)
            {
                var tokens = from token in buinessList.ElementAt(i).BusinessTokens
                             select new BusinessFrontGm.Types.BusinessTokenFrontGm()
                             {
                                 Id = token.TokenId.ToString(),
                                 Coin = token.Token.Name,
                                 Img = token.Token.Logo,
                                 TotalCoin = 10,
                             };
                //businesses.ElementAt(i).Token.Concat(tokens.ToArray());
                response.Items[i].Token.AddRange(tokens.ToArray());
            }


            return await Task.FromResult(response);
        }

        [AllowAnonymous]
        public override async Task<BusinessListFrontGm> GetFrontBusinessByLocation(BusinessFrontLocationQueryFilter request, ServerCallContext context)
        {
            BusinessListFrontGm response = new BusinessListFrontGm();

            IQueryable<Business> query = _businessRepository.GetQuery();

            string querystring = request.Query;

            var c1 = querystring.Split("#");

            //var categoryItem = c1.Where(x => x.StartsWith("category")).FirstOrDefault();
            // var categoryValue =

            var categoryValue = "saa".Split("&");
            var tokenValue = "saa".Split("&");
            for (int i=0;i<=c1.Length-1;i++)
            {
                if(c1[i].Contains("category"))
                {
                    var c2 = c1[i].Split(":");
                    categoryValue = c2[1].Split("|");
                }

                if (c1[i].Contains("token"))
                {
                    var c2 = c1[i].Split(":");
                    tokenValue = c2[1].Split("|");
                }
            }

         

            var distanceInMetres = 2000; // 2 km


            var location = new Point(request.Longitude, request.Latitude) { SRID = 4326 };

            IsValidOp isValidOp = new IsValidOp(location);

            if (isValidOp.IsValid)
            {
                query = query.Where(x => x.Location.Distance(location) <= distanceInMetres);

            }

            if (categoryValue.Length > 0)
            {
                query = query.Where(x => categoryValue.Contains(x.CategoryId.ToString()));
            }
            if (tokenValue.Length > 0)
            {
                query = query.Where(x => x.BusinessTokens.Any(c => tokenValue.Contains(c.TokenId.ToString())));
            }



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
                                 //ImageUrl = "https://pambansangbakal.com.ph/wp-content/uploads/2019/10/no-image-800x511.png",
                                 ImageUrl = getDefaultImage(business.BusinessGalleries),
                                 LocationName = "United state",
                                 Rate = 5,
                                 Description = business.Description,
                                 Name = business.Name,
                                 TotalRate = 100,
                                  

                             };

            response.Items.AddRange(businesses.ToArray());

            for (int i = 0; i < businesses.Count() - 1; i++)
            {
                var tokens = from token in buinessList.ElementAt(i).BusinessTokens
                             select new BusinessFrontGm.Types.BusinessTokenFrontGm()
                             {
                                 Id = token.TokenId.ToString(),
                                 Coin = token.Token.Name,
                                 Img = token.Token.Logo,
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
                                 ImageUrl = getDefaultImage(business.BusinessGalleries),
                             };

            response.Items.AddRange(businesses.ToArray());
            response.Pagination = pagination;
            return await Task.FromResult(response);

        }
        private string getDefaultImage(ICollection<BusinessGallery> businessGalleries)
        {
            return businessGalleries.Where(x => x.Deleted == false).Count() > 0 ?
                businessGalleries.Where(x => x.Deleted == false).FirstOrDefault().Name + businessGalleries.Where(x => x.Deleted == false).FirstOrDefault().Extension
                :
                "https://pambansangbakal.com.ph/wp-content/uploads/2019/10/no-image-800x511.png";
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

            var buinessList = await _businessRepository.GetAll(query, request.PageId, request.PageSize);



            var businesses = from business in buinessList
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
                                 ImageUrl = getDefaultImage(business.BusinessGalleries),
                             };


            
            response.Items.AddRange(businesses.ToArray());

            for (int i = 0; i < businesses.Count() - 1; i++)
            {
                var tokens = from token in buinessList.ElementAt(i).BusinessTokens
                             select new BusinessTokenFrontGm()
                             {
                                 Id = token.TokenId.ToString(),
                                 Coin = token.Token.Name,
                                 Img = token.Token.Logo,
                                 TotalCoin = 10,
                             };
                //businesses.ElementAt(i).Token.Concat(tokens.ToArray());
                response.Items[i].Tokens.AddRange(tokens.ToArray());
            }

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
                                 ImageUrl = getDefaultImage(business.BusinessGalleries),

                             };

            response.Items.AddRange(businesses.ToArray());
            response.Pagination = pagination;
            return await Task.FromResult(response);

        }
        public override async Task<BusinessListGm> GetByTagId(BusinessTagIdQueryFilter request, ServerCallContext context)
        {
            BusinessListGm response = new BusinessListGm();
            PaginationGm pagination = new PaginationGm();

            IQueryable<Business> query = _businessRepository.GetQuery();

            query = query.Where(x => x.BusinessTags.Any(x => x.TagId == Guid.Parse(request.TagId)));

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
                                 ImageUrl = getDefaultImage(business.BusinessGalleries),

                             };

            response.Items.AddRange(businesses.ToArray());
            response.Pagination = pagination;
            return await Task.FromResult(response);

        }
        public override async Task<BusinessListGm> GetByTokenId(BusinessTokenIdQueryFilter request, ServerCallContext context)
        {
            BusinessListGm response = new BusinessListGm();
            PaginationGm pagination = new PaginationGm();

            IQueryable<Business> query = _businessRepository.GetQuery();

            query = query.Where(x => x.BusinessTokens.Any(c => c.TokenId == Guid.Parse(request.TokenId)));


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
                                 ImageUrl = getDefaultImage(business.BusinessGalleries),

                             };

            response.Items.AddRange(businesses.ToArray());
            response.Pagination = pagination;
            return await Task.FromResult(response);

        }
        public override async Task<BusinessListGm> GetByCategoryId(BusinessCategoryIdQueryFilter request, ServerCallContext context)
        {
            BusinessListGm response = new BusinessListGm();
            PaginationGm pagination = new PaginationGm();

            IQueryable<Business> query = _businessRepository.GetQuery();

            query = query.Where(x => x.CategoryId == Guid.Parse(request.CategoryId));


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
                                 ImageUrl = getDefaultImage(business.BusinessGalleries),

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
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

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
                Verified = true,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                Description = request.Description,
                Address = request.Address,
                OfferedServices = request.OfferedServices,
                Location = geometryFactory.CreatePoint(new NetTopologySuite.Geometries.Coordinate(request.Longitude, request.Latitude)),
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


            foreach (var tokenGm in request.Tokens)
            {

                var token = await _tokenRepository.Find(Guid.Parse(tokenGm.TokenId));

                if (token == null)
                {
                    await _tokenRepository.Add(new Token { TokenId = Guid.Parse(tokenGm.TokenId) });
                }

                BusinessToken item = await _businessTokenRepository.Find(res.BusinessId, Guid.Parse(tokenGm.TokenId));

                if (item == null)
                {
                    var businessToken = new BusinessToken()
                    {
                        BusinessId = res.BusinessId,
                        TokenId = Guid.Parse(tokenGm.TokenId),
                    };

                    await _businessTokenRepository.Add(businessToken);
                }
                else
                {
                    await _businessTokenRepository.Delete(item);
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

        [AllowAnonymous]
        public override async Task<Empty> UpdatePoint(BusinessQueryFilter request, ServerCallContext context)
        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

            var business =await  _businessRepository.GetAll();

            foreach (var item in business)
            {
                item.Location = geometryFactory.CreatePoint(new NetTopologySuite.Geometries.Coordinate(item.Longitude, item.Latitude));
                await _businessRepository.Update(item);
            }

            return await Task.FromResult<Empty>(new Empty());
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
        public override async Task<Empty> SaveBusinessToken(BusinessTokenGm request, ServerCallContext context)
        {
            BusinessToken item = await _businessTokenRepository.Find(Guid.Parse(request.BusinessId), Guid.Parse(request.TokenId));

            if (item == null)
            {
                var businessToken = new BusinessToken()
                {
                    BusinessId = Guid.Parse(request.BusinessId),
                    TokenId = Guid.Parse(request.TokenId),
                };

                await _businessTokenRepository.Add(businessToken);
            }
            else
            {
                await _businessTokenRepository.Delete(item);
            }

            return await Task.FromResult<Empty>(new Empty());
        }
        public override async Task<Empty> SaveBusinessTag(BusinessTagGm request, ServerCallContext context)
        {
            BusinessTag item = await _businessTagRepository.Find(Guid.Parse(request.BusinessId), Guid.Parse(request.TagId));

            if (item == null)
            {
                var tag = await _tagRepository.Find(request.TagId);
                if (tag == null)
                {
                    await _tagRepository.Add(new Tag { TagId = Guid.Parse(request.TagId) });
                }
                var businessTag = new BusinessTag()
                {
                    BusinessId = Guid.Parse(request.BusinessId),
                    TagId = Guid.Parse(request.TagId),
                };

                await _businessTagRepository.Add(businessTag);
            }
            else
            {
                await _businessTagRepository.Delete(item);
            }

            return await Task.FromResult<Empty>(new Empty());
        }
        public override async Task<Empty> SaveBusinessTokenCollection(BusinessTokenListGm request, ServerCallContext context)
        {

            foreach (var entity in request.BusinessTokens)
            {
                BusinessToken item = await _businessTokenRepository.Find(Guid.Parse(entity.BusinessId), Guid.Parse(entity.TokenId));

                if (item == null)
                {
                    var businessToken = new BusinessToken()
                    {
                        BusinessId = Guid.Parse(entity.BusinessId),
                        TokenId = Guid.Parse(entity.TokenId),
                    };

                    await _businessTokenRepository.Add(businessToken);
                }
            }
            return await Task.FromResult<Empty>(new Empty());
        }
    }
}
