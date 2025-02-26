using E_Learning.Models.Request;
using E_Learning.Models.Response;

namespace E_Learning.Servies
{
    public interface ICourseService
    {
        Task<CreationCourseResponse> CreateCourse(CreationCourseRequest request);
        Task<IEnumerable<CourseResponse>> FindAll();
    }
}
