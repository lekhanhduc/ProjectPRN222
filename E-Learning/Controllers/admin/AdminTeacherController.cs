using E_Learning.Services.admin;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.Controllers.admin
{
    [ApiController]
    [Route("api/v1/admin/teachers")]
    public class AdminTeacherController : ControllerBase
    {
        private readonly IAdminTeacherService _teacherService;

        public AdminTeacherController(IAdminTeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpGet]
        public IActionResult GetAllTeachers(int page = 1, int size = 10, string sortBy = "Name", bool isDescending = false)
        {
            var teachers = _teacherService.GetAllTeachers(page, size, sortBy, isDescending);
            return Ok(teachers);
        }

        [HttpGet("search")]
        public IActionResult SearchTeachers(string keywords, int page = 1, int size = 10, string sortBy = "Name", bool isDescending = false)
        {
            var teachers = _teacherService.SearchTeachers(keywords, page, size, sortBy, isDescending);
            return Ok(teachers);
        }

        [HttpPut("{userId}/remove-role")]
        public IActionResult RemoveTeacherRole(long userId)
        {
            _teacherService.RemoveTeacherRole(userId);
            return Ok("Role updated successfully.");
        }

        [HttpGet("applications")]
        public IActionResult GetPendingTeacherApplications(int page = 1, int size = 10, string sortBy = "Name", bool isDescending = false)
        {
            var applications = _teacherService.GetPendingTeacherApplications(page, size, sortBy, isDescending);
            return Ok(applications);
        }
    }
}
