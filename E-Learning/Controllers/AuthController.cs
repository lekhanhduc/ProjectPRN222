using E_Learning.Dto.Request;
using E_Learning.Dto.Response;
using E_Learning.Models.Request;
using E_Learning.Models.Response;
using E_Learning.Servies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthenticationService authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        [HttpPost("sign-in")]
        [AllowAnonymous]
        public async Task<ApiResponse<SignInResponse>> SigIn([FromBody] SignInRequest request)
        {
            var result = await authenticationService.SignIn(request);

            return new ApiResponse<SignInResponse>
            {
                code = 200,
                message = "SignIn Successfully",
                result = result
            };
        }

        [HttpPost("outbound/authentication")]
        [AllowAnonymous]
        public async Task<ApiResponse<SignInResponse>> SigInGoogle([FromQuery] string code)
        {
            var result = await authenticationService.SignInWithGoogle(code);

            return new ApiResponse<SignInResponse>
            {
                code = 200,
                message = "SignIn Google Successfully",
                result = result
            };
        }

        [HttpPost("introspect")]
        public async Task<ApiResponse<IntrospectResponse>> Introspect([FromBody] IntrospectRequest request)
        {
            var result = await authenticationService.VerifyToken(request);
            return new ApiResponse<IntrospectResponse>
            {
                code = 200,
                message = "Introspect Successfully",
                result = result
            };
        }

        [HttpPost("refresh-token")]
        public async Task<ApiResponse<SignInResponse>> RefreshToken()
        {
            var result = await authenticationService.RefreshToken();
            return new ApiResponse<SignInResponse>
            {
                code = 200,
                message = "Refresh Token Successfully",
                result = result
            };
        }

        [HttpPost("logout")]
        public async Task<ApiResponse<object>> Logout()
        {
            await authenticationService.SignOut();
            return new ApiResponse<object>
            {
                code = 200,
                message = "Sign Out Successfully"
            };
        }

    }
}
