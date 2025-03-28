using E_Learning.Dto.Response;
using E_Learning.Models.Response;

namespace E_Learning.Services.admin
{
    public interface IAdminUserService
    {
        PagedResult<UserResponse> GetAllUsers(int page, int size, string sortBy, bool isDescending);
        PagedResult<UserResponse> SearchUsers(string keywords, int page, int size, string sortBy, bool isDescending);
        void BanUser(long userId);
        void UnbanUser(long userId);
        void UpdateUserRole(long userId, string roleName);
        //TeacherApplicationDetailResponse GetUserApplicationDetail(long userId);
    }
}
