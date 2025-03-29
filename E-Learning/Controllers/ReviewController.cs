using E_Learning.Dto.Request;
using E_Learning.Dto.Response;
using E_Learning.Models.Response;
using E_Learning.Servies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.Controllers
{
    [Route("api/v1/review")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService reviewService;
        public ReviewController(IReviewService reviewService)
        {
            this.reviewService = reviewService;
        }


        [HttpPost]
        public async Task<ApiResponse<ReviewResponse>> AddReview([FromBody] ReviewRequest request)
        {
            var result = await reviewService.AddReview(request);
            return new ApiResponse<ReviewResponse>
            {
                code = 201,
                message = "Created Review Success",
                result = result
            };
        }

        [HttpGet("{courseId}")]
        public async Task<ApiResponse<List<ReviewResponse>>> GetReviewByCourse(long courseId)
        {
            var result = await reviewService.GetReviewByCourse(courseId);
            return new ApiResponse<List<ReviewResponse>>
            {
                code = 200,
                message = "Get Review Success",
                result = result
            };
        }

        [HttpDelete("{reviewId}")]
        public async Task<ApiResponse<DeleteReviewResponse>> DeleteReview(long reviewId)
        {
            var result = await reviewService.DeleteReview(reviewId);
            return new ApiResponse<DeleteReviewResponse>
            {
                code = 200,
                message = "Delete Review Success",
                result = result
            };
        }

    }
}
