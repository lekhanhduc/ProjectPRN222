using E_Learning.Dto.Response;
using E_Learning.Models.Response;

namespace E_Learning.Services.admin
{
    public interface IAdminTeacherService
    {
        PagedResult<UserResponse> GetAllTeachers(int page, int size, string sortBy, bool isDescending);
        PagedResult<UserResponse> SearchTeachers(string keywords, int page, int size, string sortBy, bool isDescending);
        void RemoveTeacherRole(long userId);
        PagedResult<UserResponse> GetPendingTeacherApplications(int page, int size, string sortBy, bool isDescending);
    }
}
