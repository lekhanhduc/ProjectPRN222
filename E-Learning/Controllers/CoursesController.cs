using E_Learning.Dto.Request;
using E_Learning.Dto.Response;
using E_Learning.Dto.Response.E_Learning.Dto.Response;
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


        [Authorize(Roles = "TEACHER")]
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

        [HttpGet("search")]
        public async Task<ApiResponse<PageResponse<CourseResponse>>> SearchCourses
            (
            [FromQuery] int? page,
            [FromQuery] int? size,
            [FromQuery] string? keyword,
            [FromQuery] string? level,
            [FromQuery] double? minPrice,
            [FromQuery] double? maxPrice
            )
        {
            int currentPage = page ?? 1;
            int pageSize = size ?? 6;
            var result = await courseService.GetAllWithSearchElastic(currentPage, pageSize, keyword, level, minPrice, maxPrice);
            return new ApiResponse<PageResponse<CourseResponse>>(
                code: 200,
                message: "Search Courses",
                result: result
                );
        }

        [Authorize(Roles = "TEACHER")]
        [HttpGet("teacher")]
        public async Task<ApiResponse<PageResponse<CourseResponse>>> GetMyCourses(
                [FromQuery] int page = 1,
                [FromQuery] int size = 4) 
        {
            var result = await courseService.GetCourseByTeacher(page, size);

            return new ApiResponse<PageResponse<CourseResponse>>
            {
                code = 200,
                message = "Get courses by teacher successfully",
                result = result
            };
        }

        [HttpGet("overview/{courseId}")]
        public async Task<ApiResponse<OverviewCourseResponse>> OverviewCourseDetail(long courseId)
        {
            var result = await courseService.OverviewCourseDetail(courseId);
            return new ApiResponse<OverviewCourseResponse>
            {
                code = 200,
                message = "Get overview course successfully",
                result = result
            };
        }

        [HttpGet("info/{courseId}")]
        public async Task<ApiResponse<CourseChapterResponse>> GetAllInfoCourse(long courseId)
        {
            var result = await courseService.GetAllInfoCourse(courseId);
            return new ApiResponse<CourseChapterResponse>
            {
                code = 200,
                message = "Get all info course successfully",
                result = result
            };
        }

        [HttpGet("purchase/{courseId}")]
        public async Task<ApiResponse<CoursePurchaseResponse>> CheckPurchase(long courseId)
        {
            var result = await courseService.CheckPurchase(courseId);
            return new ApiResponse<CoursePurchaseResponse>
            {
                code = 200,
                message = "Check purchase successfully",
                result = result
            };
        }

        [HttpGet("info-detail/{courseId}")]
        public async Task<ApiResponse<CourseChapterResponse>> InfoCourse(long courseId)
        {
            var result = await courseService.InfoCourse(courseId);
            return new ApiResponse<CourseChapterResponse>
            {
                code = 200,
                message = "Get info course successfully",
                result = result
            };
        }

    }
}
