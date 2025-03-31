using E_Learning.Dto.Response;
using E_Learning.Models.Response;
using E_Learning.Servies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService teacherService;
        public TeacherController(ITeacherService teacherService)
        {
            this.teacherService = teacherService;
        }

        [HttpGet("info-student")]
        [Authorize(Roles = "TEACHER")]
        public async Task<ApiResponse<PageResponse<StudentResponse>>> GetStudentsByPurchasedCourses([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var result = await teacherService.GetStudentsByPurchasedCourses(page, size);

            return new ApiResponse<PageResponse<StudentResponse>>
            {
                code = 200,
                message = "Success",
                result = result
            };
        }

    }
}
