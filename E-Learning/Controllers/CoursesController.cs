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

        [HttpGet("fetch-all")]
        public async Task<ApiResponse<IEnumerable<CourseResponse>>> FetchAll()
        {
            var result = await courseService.FindAll();

            return new ApiResponse<IEnumerable<CourseResponse>>(
                code: 200,
                message: "Fetch All Courses",
                result: result
                );
        }

    }
}
