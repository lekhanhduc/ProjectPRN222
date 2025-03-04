using E_Learning.Dto.Request;
using E_Learning.Dto.Response;
using E_Learning.Models.Response;

namespace E_Learning.Servies
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseResponse>> FindAll();
        Task<CourseCreationResponse> CreateCourse(CourseCreationRequest request);
    }
}
