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

    }
}
