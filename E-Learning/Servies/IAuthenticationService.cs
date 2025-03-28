using E_Learning.Dto.Request;
using E_Learning.Dto.Response;
using E_Learning.Models.Request;
using E_Learning.Models.Response;

namespace E_Learning.Servies
{
    public interface IAuthenticationService
    {
        Task<SignInResponse> SignIn(SignInRequest request);

        Task<SignInResponse> SignInWithGoogle(string code);
        Task<IntrospectResponse> VerifyToken(IntrospectRequest request);

        Task<IntrospectResponse> VerifyRefreshToken(string token);

        Task<SignInResponse> RefreshToken();
        Task SignOut();
    }
}
