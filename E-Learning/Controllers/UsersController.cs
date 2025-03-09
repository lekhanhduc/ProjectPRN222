using E_Learning.Dto.Response;
using E_Learning.Models.Request;
using E_Learning.Models.Response;
using E_Learning.Servies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.Controllers
{
    [Route("api/v1/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }


        [Authorize(Roles = "USER")]
        [HttpGet]
        public async Task<ApiResponse<PageResponse<UserResponse>>> FetchAllUser
            (
                [FromBody] int? page,
                [FromBody] int? size
            )
        {
            int currentPage = page ?? 1;
            int pageSize = size ?? 4;

            var users = await userService.FindAll(currentPage, pageSize);

            return new ApiResponse<PageResponse<UserResponse>>(
                code: 200,
                message: "Fetch All Users",
                result: users
            );
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiResponse<UserCreationResponse>> CreateUser([FromBody] UserCreationRequest request)
        {
            var users = await userService.CreateUser(request);

            return new ApiResponse<UserCreationResponse>(
                code: 201,
                message: "Created user",
                result: users
            );
        }

        [HttpGet("verification")]
        [AllowAnonymous]
        public async Task<ApiResponse<VerificationResponse>> Verification([FromQuery] string email, [FromQuery] string otp)
        {
            var result = await userService.Verification(email, otp);

            return new ApiResponse<VerificationResponse>(
                code: 201,
                message: "Verfication success",
                result: result
            );
        }

        [Authorize]
        [HttpGet("my-info")]
        public async Task<ApiResponse<UserResponse>> MyInfo()
        {
            var result = await userService.MyInfo();
            return new ApiResponse<UserResponse>(
                code: 200,
                message: "My Info",
                result: result
            );
        }

    }
}
