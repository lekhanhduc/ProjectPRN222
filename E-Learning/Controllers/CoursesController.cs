using E_Learning.Dto.Request;
using E_Learning.Dto.Response;
using E_Learning.Models.Response;
using E_Learning.Servies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.Controllers
{
    [Route("api/v1/courses")]
    [ApiController]
    public class CoursesController : ControllerBase
    {

        private readonly ICourseService courseService;

        public CoursesController(ICourseService courseService)
        {
            this.courseService = courseService;
        }


        [Authorize]
        [HttpPost]
        public async Task<ApiResponse<CourseCreationResponse>> CreateCourse([FromForm] CourseCreationRequest request)
        {
            var result = await courseService.CreateCourse(request);

            return new ApiResponse<CourseCreationResponse>
            {
                code = 201,
                message = "Created course",
                result = result
            };
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ApiResponse<PageResponse<CourseResponse>>> FetchAll
            (
            [FromQuery] int? page,
            [FromQuery] int? size
            )
        {
            int currentPage = page ?? 1;
            int pageSize = size ?? 4;

            var result = await courseService.FindAll(currentPage, pageSize);

            return new ApiResponse<PageResponse<CourseResponse>>(
                code: 200,
                message: "Fetch All Courses",
                result: result
                );
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ApiResponse<CourseResponse>> FetchById(int id)
        {
            var result = await courseService.FindById(id);
            return new ApiResponse<CourseResponse>
            {
                code = 200,
                message = "Fetch Course By Id",
                result = result
            };
        }

    }
}
