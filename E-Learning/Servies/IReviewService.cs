using E_Learning.Dto.Request;
using E_Learning.Dto.Response;

namespace E_Learning.Servies
{
    public interface IReviewService
    {
        Task<List<ReviewResponse>> GetReviewByCourse(long courseId);
        Task<ReviewResponse> AddReview(ReviewRequest request);
        Task<DeleteReviewResponse> DeleteReview(long reviewId);
    }
}
