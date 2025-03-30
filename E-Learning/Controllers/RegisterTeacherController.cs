using E_Learning.Dto.Request;
using E_Learning.Dto.Response;
using E_Learning.Models.Response;
using E_Learning.Servies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class RegisterTeacherController : ControllerBase
    {
        private readonly RegisterTeacherService _registerTeacherService;

        public RegisterTeacherController(RegisterTeacherService registerTeacherService)
        {
            _registerTeacherService = registerTeacherService;
        }

        [HttpGet("registration-teachers")]
        public async Task<ApiResponse<List<UserRegisterTeacherResponse>>> GetAll()
        {
            var result = await _registerTeacherService.GetAll();
            return new ApiResponse<List<UserRegisterTeacherResponse>>()
            {
                code = 200,
                result = result
            };
        }

        [HttpPost("register-teacher")]
        public async Task<ApiResponse<UserRegisterTeacherResponse>> RegisterTeacher(
            [FromForm] UserRegisterTeacherRequest request
           )
        {
            var result = await _registerTeacherService.RegisterTeacher(request);
            return new ApiResponse<UserRegisterTeacherResponse>()
            {
                code = 200,
                result = result
            };
        }

        [HttpPost("save-teacher/{id}")]
        public async Task<ApiResponse<UserRegisterTeacherResponse>> SaveTeacher([FromRoute] long id)
        {
            var result = await _registerTeacherService.ApproveTeacher(id);
            return new ApiResponse<UserRegisterTeacherResponse>()
            {
                code = 201,
                message = "Approve Teacher Successfully",
                result = result
            };
        }

        [HttpPost("reject-teacher/{id}")]
        public async Task<ApiResponse<UserRegisterTeacherResponse>> RejectTeacher([FromRoute] long id)
        {
            var result = await _registerTeacherService.RejectTeacher(id);
            return new ApiResponse<UserRegisterTeacherResponse>()
            {
                code = 200,
                message = "Reject Successfully",
                result = result
            };
        }
    }
}
