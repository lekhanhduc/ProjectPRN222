using E_Learning.Dto.Response;
using E_Learning.Dto.Response.Admin;
using E_Learning.Servies.admin;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.Controllers.admin
{
    [Route("api/v1/admin/courses")]
    [ApiController]
    public class AdminCourseController : ControllerBase
    {
        private readonly AdminCourseService _courseService;

        public AdminCourseController(AdminCourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<AdminCourseResponse>>> GetAllCourses([FromQuery] int page = 1, [FromQuery] int size = 10, [FromQuery] string sort = "title,asc")
        {
            var sortArgs = sort.Split(',');
            string sortBy = sortArgs[0];
            string sortDir = sortArgs.Length > 1 ? sortArgs[1] : "asc";
            var courses = await _courseService.GetAllCourses(page, size, sortBy, sortDir);
            return Ok(courses);
        }

        [HttpGet("search")]
        public async Task<ActionResult<PagedResult<AdminCourseResponse>>> SearchCourses([FromQuery] string keywords, [FromQuery] int page = 1, [FromQuery] int size = 10, [FromQuery] string sort = "title,asc")
        {
            var keywordsArray = keywords.Split(' ');
            // Assuming you have a method to search by keywords.
            var courses = await _courseService.SearchCoursesByKeywords(keywordsArray, page, size, sort);
            return Ok(courses);
        }

        [HttpPost("{courseId}/ban")]
        public async Task<ActionResult> BanCourse(long courseId)
        {
            await _courseService.BanCourse(courseId);
            return Ok("Course banned successfully.");
        }

        [HttpPost("{courseId}/unban")]
        public async Task<ActionResult> UnbanCourse(long courseId)
        {
            await _courseService.UnbanCourse(courseId);
            return Ok("Course unbanned successfully.");
        }

        [HttpGet("banned")]
        public async Task<ActionResult<PagedResult<AdminCourseResponse>>> GetBannedCourses([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var courses = await _courseService.GetBannedCourses(page, size);
            return Ok(courses);
        }

        [HttpGet("active")]
        public async Task<ActionResult<PagedResult<AdminCourseResponse>>> GetActiveCourses([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var courses = await _courseService.GetActiveCourses(page, size);
            return Ok(courses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AdminCourseResponse>> GetCourseDetails(long id)
        {
            var response = await _courseService.GetCourseDetails(id);
            return Ok(response);
        }
    }
}
