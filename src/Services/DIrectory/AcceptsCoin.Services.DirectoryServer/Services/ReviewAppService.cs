using System;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.Services.DirectoryServer.Core.Interfaces;
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
    public class ReviewGrpcService : ReviewAppService.ReviewAppServiceBase
    {
        private readonly ILogger<ReviewGrpcService> _logger;
        private IReviewRepository _ReviewRepository;
        public ReviewGrpcService(ILogger<ReviewGrpcService> logger, IReviewRepository ReviewRepository)
        {
            _logger = logger;
            _ReviewRepository = ReviewRepository;
        }

        private Guid getUserId(ServerCallContext context)
        {
            return Guid.Parse(context.GetHttpContext().User.Identity.Name);
        }
        private string getPartnetId(ServerCallContext context)
        {
            return "bff3b2dd-e89d-46fc-a868-aab93a3efbbe";
        }
        public override async Task<ReviewListGm> GetAll(ReviewQueryFilter request, ServerCallContext context)
        {
            ReviewListGm response = new ReviewListGm();

            IQueryable<Review> query = _ReviewRepository.GetQuery();


            response.CurrentPage = request.PageId;
            response.ItemCount = await _ReviewRepository.GetCount(query);
            response.PageCount = (response.ItemCount / request.PageSize) + 1;


            var Reviews = from Review in await _ReviewRepository.GetAll(query, request.PageId, request.PageSize)
                          select new ReviewGm()
                          {
                              Id = Review.ReviewId.ToString(),
                              Message = Review.Message,
                              Rate = Review.Rate,

                          };
            response.Items.AddRange(Reviews.ToArray());
            return await Task.FromResult(response);
        }


        [AllowAnonymous]
        public override async Task<ReviewListFrontGm> GetFrontReviewByBusinessId(ReviewBusinessQueryFilter request, ServerCallContext context)
        {
            ReviewListFrontGm response = new ReviewListFrontGm();

            IQueryable<Review> query = _ReviewRepository.GetQuery();
            query = query.Where(x => x.BusinessId == Guid.Parse(request.BusinessId) && x.Deleted == false);

            response.CurrentPage = request.PageId;
            response.ItemCount = await _ReviewRepository.GetCount(query);
            response.PageCount = (response.ItemCount / request.PageSize) + 1;


            var Reviews = from Review in await _ReviewRepository.GetAll(query, request.PageId, request.PageSize)
                          select new ReviewFrontGm()
                          {
                              Id = Review.ReviewId.ToString(),
                              Message = Review.Message,
                              Rate = Review.Rate,

                          };
            response.Items.AddRange(Reviews.ToArray());
            return await Task.FromResult(response);
        }

        public override async Task<ReviewGm> GetById(ReviewIdFilter request, ServerCallContext context)
        {
            var Review = await _ReviewRepository.Find(Guid.Parse(request.ReviewId));
            var searchedReview = new ReviewGm()
            {
                Id = Review.ReviewId.ToString(),
                Message = Review.Message,
                Rate = Review.Rate,

            };
            return await Task.FromResult(searchedReview);
        }

        public override async Task<ReviewGm> Post(ReviewGm request, ServerCallContext context)
        {

            var reviewAdded = new Review()
            {
                ReviewId = Guid.NewGuid(),
                Message = request.Message,
                Rate=request.Rate,
                CreatedById = getUserId(context),
                CreatedDate = DateTime.Now,
                Published = true,
                Deleted = false,
            };

            var res = await _ReviewRepository.Add(reviewAdded);

            var response = new ReviewGm()
            {
                Id = res.ReviewId.ToString(),
                Message = res.Message,
                Rate = res.Rate,
            };
            return await Task.FromResult(response);
        }


        public override async Task<ReviewGm> Put(ReviewGm request,
           ServerCallContext context)
        {
            Review review = await _ReviewRepository.Find(Guid.Parse(request.Id));
            if (review == null)
            {
                return await Task.FromResult<ReviewGm>(null);
            }


            review.Message = request.Message;
            review.Rate = request.Rate;
            review.UpdatedById = getUserId(context);
            review.UpdatedDate = DateTime.Now;





            await _ReviewRepository.Update(review);
            return await Task.FromResult<ReviewGm>(new ReviewGm()
            {
                Id = review.ReviewId.ToString(),
                Message = review.Message,
                Rate= review.Rate,
            });
        }



        public override async Task<EmptyReview> Delete(ReviewIdFilter request, ServerCallContext context)
        {
            Review review = await _ReviewRepository.Find(Guid.Parse(request.ReviewId));
            if (review == null)
            {
                return await Task.FromResult<EmptyReview>(null);
            }

            await _ReviewRepository.Delete(review);
            return await Task.FromResult<EmptyReview>(new EmptyReview());
        }

        public override async Task<EmptyReview> SoftDelete(ReviewIdFilter request, ServerCallContext context)
        {
            Review Review = await _ReviewRepository.Find(Guid.Parse(request.ReviewId));

            if (Review == null)
            {
                return await Task.FromResult<EmptyReview>(null);
            }

            Review.Deleted = true;
            Review.UpdatedById = getUserId(context);
            Review.UpdatedDate = DateTime.Now;

            await _ReviewRepository.Update(Review);
            return await Task.FromResult<EmptyReview>(new EmptyReview());
        }
        public override async Task<EmptyReview> SoftDeleteCollection(ReviewDeleteCollectionGm request, ServerCallContext context)
        {

            foreach (var item in request.Items)
            {
                Review Review = await _ReviewRepository.Find(Guid.Parse(item.ReviewId));

                if (Review == null)
                {
                    return await Task.FromResult<EmptyReview>(null);
                }

                Review.Deleted = true;
                Review.UpdatedById = getUserId(context);
                Review.UpdatedDate = DateTime.Now;

                await _ReviewRepository.Update(Review);
            }

            return await Task.FromResult<EmptyReview>(new EmptyReview());
        }
    }
}
