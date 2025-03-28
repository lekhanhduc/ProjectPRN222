using E_Learning.Dto.Request;
using E_Learning.Dto.Response;

namespace E_Learning.Servies
{
    public interface IProfileService
    {
        Task<UserProfileResponse> GetUserProfile();
        Task UpdateUserProfile(UserProfileRequest request);
        Task<string?> GetAvatar();
        Task<string?> UpdateAvatar(UpdateAvatarRequest request);
        Task RomoveAvatar();   
    }
}
