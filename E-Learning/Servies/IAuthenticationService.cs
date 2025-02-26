using E_Learning.Models.Request;
using E_Learning.Models.Response;

namespace E_Learning.Servies
{
    public interface IAuthenticationService
    {
        Task<SignInResponse> SignIn(SignInRequest request);
    }
}
