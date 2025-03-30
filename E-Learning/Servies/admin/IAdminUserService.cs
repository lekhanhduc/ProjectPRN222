using E_Learning.Dto.Response;
using E_Learning.Dto.Response.admin;
using E_Learning.Models.Response;
using PagedList;

namespace E_Learning.Services.admin
{
    public interface IAdminUserService
    {
        Task<PaginatedList<AdminUserResponse>> GetAllUsersAsync(int page, int size, string sortBy, string sortDir);
        Task BanUserAsync(long userId);
        Task UnbanUserAsync(long userId);
        Task UpdateUserRoleAsync(long userId, string roleName);
        Task<TeacherApplicationDetailResponse> GetUserDetailAsync(long userId);

        // Tìm kiếm người dùng theo từ khóa
        Task<PaginatedList<AdminUserResponse>> SearchUsersByKeywordsAsync(string[] keywords, int page, int size, string sortBy, string sortDir);
    }
}
