using E_Learning.Dto.Response.admin;

namespace E_Learning.Servies.admin
{
    public interface IAdminReviewService
    {
        Task<List<AdminReviewDTO>> GetReviewsByCourseAndRatingAsync(int month, int year, bool ascending);
    }
}
