using E_Learning.Dto.Request;
using E_Learning.Dto.Response;
using E_Learning.Entity;
using E_Learning.Middlewares;
using E_Learning.Repositories;
using Elastic.Clients.Elasticsearch;
using System.Threading.Tasks;

namespace E_Learning.Servies.Impl
{
    public class ReviewService: IReviewService
    {
        private readonly ReviewRepository reviewRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly CourseRepository courseRepository;
        private readonly UserRepository userRepository;

        public ReviewService(ReviewRepository reviewRepository, IHttpContextAccessor httpContextAccessor, CourseRepository courseRepository,
            UserRepository userRepository)
        {
            this.reviewRepository = reviewRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.courseRepository = courseRepository;
            this.userRepository = userRepository;
        }

        public async Task<List<ReviewResponse>> GetReviewByCourse(long courseId)
        {
            var allReviews = await reviewRepository
                                 .FindByCourseIdAndChapterIsNullAndLessonIsNullAsync(courseId);

            Console.WriteLine(allReviews);
            var reviews = allReviews
                            .Where(comment => comment.ParentReview == null)
                            .Select(review => new ReviewResponse
                            {
                                Id = review.Id,
                                Name = review.User.Name,
                                Avatar = review.User.Avatar,
                                Content = review.Content,
                                Rating = review.Rating,
                                CreatedAt = review.CreatedAt,
                                UpdatedAt = review.UpdatedAt,
                                Replies = allReviews
                                            .Where(reply => reply.ParentReviewId == review.Id)
                                            .Select(reply => new ReviewResponse
                                            {
                                                Id = reply.Id,
                                                Name = reply.User.Name,
                                                Avatar = reply.User.Avatar,
                                                Content = reply.Content,
                                                Rating = reply.Rating,
                                                CreatedAt = reply.CreatedAt,
                                                UpdatedAt = reply.UpdatedAt,
                                                Replies = new List<ReviewResponse>() 
                                            })
                                            .ToList()
                            })
                            .ToList();

            return reviews;
        }


        public async Task<ReviewResponse> AddReview(ReviewRequest request)
        {
            var userId = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userId == null)
            {
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }

            var user = await userRepository.FindUserById(long.Parse(userId.Value));
            if (user == null)
            {
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }

            var course = await courseRepository.FindById(request.CourseId);
            if (course == null)
            {
                throw new AppException(ErrorCode.COURSE_NOT_EXISTED);
            }

            Review parentReview = null;

            if (request.ParentReviewId != null)
            {
                parentReview = await reviewRepository.FindByIdAsync(request.ParentReviewId.Value)
                                ?? throw new AppException(ErrorCode.PARENT_COMMENT_NOT_EXISTED);
            }

            if ((string.IsNullOrEmpty(request.Content) && request.Rating == null))
            {
                throw new AppException(ErrorCode.INVALID_COMMENT_OR_RATING);
            }

            if (request.Rating != null && (request.Rating < 0 || request.Rating > 5))
            {
                throw new AppException(ErrorCode.INVALID_RATING);
            }

            var newComment = new Review
            {
                User = user,
                Content = !string.IsNullOrEmpty(request.Content) ? request.Content : string.Empty,
                Rating = request.Rating,
                Course = course,
                ParentReview = parentReview
            };

            await reviewRepository.Add(newComment);

            var reviewResponse = new ReviewResponse
            {
                Id = newComment.Id,
                Name = newComment.User.Name,
                Avatar = newComment.User.Avatar,
                Content = newComment.Content,
                Rating = newComment.Rating,
                CreatedAt = newComment.CreatedAt,
                UpdatedAt = newComment.UpdatedAt,
                Replies = new List<ReviewResponse>()
            };

            return reviewResponse;
        }

        public async Task<DeleteReviewResponse> DeleteReview(long reviewId)
        {
            var userId = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userId == null)
            {
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }

            var user = await userRepository.FindUserById(long.Parse(userId.Value));
            if (user == null)
            {
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }
            var review =await reviewRepository.GetById(reviewId)
                            ?? throw new AppException(ErrorCode.REVIEW_NOT_EXISTED);

            if (user.Id == review.User.Id)
            {
                await reviewRepository.Delete(review); 
                return new DeleteReviewResponse
                {
                    Id = reviewId,
                    Message = "Delete Comment Successfully"
                };
            }

            throw new AppException(ErrorCode.DELETE_COMMENT_INVALID);
        }
    }
}
