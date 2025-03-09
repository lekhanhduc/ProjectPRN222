using E_Learning.Dto.Response;
using E_Learning.Models.Request;
using E_Learning.Models.Response;

namespace E_Learning.Servies
{
    public interface IUserService
    {
        Task<PageResponse<UserResponse>> FindAll(int pgae, int size);
        Task<UserCreationResponse> CreateUser(UserCreationRequest request);
        Task<VerificationResponse> Verification(string email, string otp);
        Task<UserResponse> MyInfo();
    }
}
