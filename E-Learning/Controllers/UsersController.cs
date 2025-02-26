using E_Learning.Dto.Response;
using E_Learning.Models.Request;
using E_Learning.Models.Response;
using E_Learning.Servies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("find-all")]
        public async Task<ResponseData<IEnumerable<UserResponse>>> FetchAllUser()
        {
            var users = await userService.FindAll();

            return new ResponseData<IEnumerable<UserResponse>>(
                code: 200,
                message: "Fetch All Users",
                data: users
            );
        }

        [HttpPost("create")]
        [AllowAnonymous] // Bỏ qua xác thực cho endpoint này
        public async Task<ResponseData<UserCreationResponse>> CreateUser([FromBody] UserCreationRequest request)
        {
            var users = await userService.CreateUser(request);

            return new ResponseData<UserCreationResponse>(
                code: 201,
                message: "Created user",
                data: users
            );
        }

        [HttpGet("verification")]
        [AllowAnonymous] // Bỏ qua xác thực cho endpoint này
        public async Task<ResponseData<VerificationResponse>> Verification([FromQuery] string email, [FromQuery] string otp)
        {
            var result = await userService.Verification(email, otp);

            return new ResponseData<VerificationResponse>(
                code: 201,
                message: "Verfication success",
                data: result
            );
        }

    }
}
