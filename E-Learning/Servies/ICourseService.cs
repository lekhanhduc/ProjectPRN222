using E_Learning.Dto.Request;
using E_Learning.Dto.Response;
using E_Learning.Models.Response;

namespace E_Learning.Servies
{
    public interface ICourseService
    {
        Task<PageResponse<CourseResponse>> FindAll(int page, int size);
        Task<CourseCreationResponse> CreateCourse(CourseCreationRequest request);
        Task<CourseResponse> FindById(int id);
    }
}
