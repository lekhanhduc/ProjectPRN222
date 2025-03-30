using E_Learning.Services.admin;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.Controllers.admin
{
    [Route("api/v1/admin/teachers")]
    [ApiController]
    public class AdminTeacherController : ControllerBase
    {
        private readonly AdminTeacherService _teacherService;
        private readonly ILogger<AdminTeacherController> _logger;

        public AdminTeacherController(AdminTeacherService teacherService, ILogger<AdminTeacherController> logger)
        {
            _teacherService = teacherService;
            _logger = logger;
        }

        // Get all teachers
        [HttpGet]
        public async Task<IActionResult> GetAllTeachers([FromQuery] int page = 1, [FromQuery] int size = 10, [FromQuery] string sort = "name,asc")
        {
            var sortArgs = sort.Split(',');
            string sortBy = sortArgs[0];
            string sortDir = sortArgs.Length > 1 ? sortArgs[1] : "asc";

            var teachers = await _teacherService.GetAllTeachers(page, size, sortBy, sortDir);
            return Ok(teachers);
        }

        // Search teachers by keywords
        [HttpGet("search")]
        public async Task<IActionResult> SearchTeachersByKeywords([FromQuery] string keywords, [FromQuery] int page = 1, [FromQuery] int size = 10, [FromQuery] string sort = "name,asc")
        {
            string[] keywordArray = keywords.Split(' ');
            var sortArgs = sort.Split(',');
            string sortBy = sortArgs[0];
            string sortDir = sortArgs.Length > 1 ? sortArgs[1] : "asc";

            var teachers = await _teacherService.SearchTeachersByKeywords(keywordArray, page, size, sortBy, sortDir);
            return Ok(teachers);
        }

        // Remove teacher role
        [HttpPut("{userId}/remove-role")]
        public async Task<IActionResult> RemoveTeacherRole([FromRoute] long userId)
        {
            await _teacherService.RemoveTeacherRole(userId);
            return Ok("User role updated to USER and registration status set to null successfully");
        }

        // Get pending teacher applications
        [HttpGet("applications")]
        public async Task<IActionResult> GetPendingTeacherApplications([FromQuery] int page = 1, [FromQuery] int size = 10, [FromQuery] string sort = "name,asc")
        {
            var sortArgs = sort.Split(',');
            string sortBy = sortArgs[0];
            string sortDir = sortArgs.Length > 1 ? sortArgs[1] : "asc";

            var applications = await _teacherService.GetPendingTeacherApplications(page, size, sortBy, sortDir);
            return Ok(applications);
        }
    }
}
