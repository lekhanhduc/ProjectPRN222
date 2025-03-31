using E_Learning.Data;
using E_Learning.Dto.Response.admin;
using E_Learning.Repositories;

namespace E_Learning.Servies.admin
{
    public class AdminReviewService : IAdminReviewService
    {
        private readonly ReviewRepository _reviewRepository;

        public AdminReviewService(ReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<List<AdminReviewDTO>> GetReviewsByCourseAndRatingAsync(int month, int year, bool ascending)
        {
            var startDate = new DateTime(year, month, 1, 0, 0, 0);
            var endDate = startDate.AddMonths(1).AddSeconds(-1); // đến hết ngày cuối cùng của tháng

            var reviews = await _reviewRepository.FindAverageRatingForCoursesInMonthYearAsync(startDate, endDate);

            if (ascending)
            {
                reviews = reviews.OrderBy(r => r.AverageRating).ToList();
            }
            else
            {
                reviews = reviews.OrderByDescending(r => r.AverageRating).ToList();
            }

            return reviews;
        }
    }
}
