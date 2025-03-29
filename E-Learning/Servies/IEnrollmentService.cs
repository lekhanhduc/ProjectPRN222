using E_Learning.Dto.Request;
using E_Learning.Dto.Response;

namespace E_Learning.Servies
{
    public interface IEnrollmentService
    {
        Task<IsCourseCompleteResponse> IsCompleteCourse(IsCourseCompleteRequest request);
        Task<List<BuyCourseResponse>> GetCourseByUserCurrent();
    }
}
