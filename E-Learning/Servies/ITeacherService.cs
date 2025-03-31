using E_Learning.Dto.Response;

namespace E_Learning.Servies
{
    public interface ITeacherService
    {
        Task<PageResponse<StudentResponse>> GetStudentsByPurchasedCourses(int page, int size);
    }
}
