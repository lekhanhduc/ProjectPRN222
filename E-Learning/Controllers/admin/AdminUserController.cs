using Microsoft.AspNetCore.Mvc;
using E_Learning.Servies;
using E_Learning.Models.Response;
using E_Learning.Services.admin;

namespace E_Learning.Controllers.admin
{
    [ApiController]
    [Route("api/v1/admin/users")]
    public class AdminUserController : ControllerBase
    {
        private readonly IAdminUserService _userService;

        public AdminUserController(IAdminUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetAllUsers(int page = 1, int size = 10, string sortBy = "Name", bool isDescending = false)
        {
            var users = _userService.GetAllUsers(page, size, sortBy, isDescending);
            return Ok(users);
        }

        [HttpGet("search")]
        public IActionResult SearchUsers(string keywords, int page = 1, int size = 10, string sortBy = "Name", bool isDescending = false)
        {
            var users = _userService.SearchUsers(keywords, page, size, sortBy, isDescending);
            return Ok(users);
        }

        [HttpPost("ban/{userId}")]
        public IActionResult BanUser(long userId)
        {
            _userService.BanUser(userId);
            return Ok("User banned successfully");
        }

        [HttpPost("unban/{userId}")]
        public IActionResult UnbanUser(long userId)
        {
            _userService.UnbanUser(userId);
            return Ok("User unbanned successfully");
        }

        [HttpPut("{userId}/role")]
        public IActionResult UpdateUserRole(long userId, [FromQuery] string roleName)
        {
            _userService.UpdateUserRole(userId, roleName);
            return Ok("User role updated successfully");
        }

        //[HttpGet("{userId}/details")]
        //public IActionResult GetUserDetails(long userId)
        //{
        //    var details = _userService.GetUserApplicationDetail(userId);
        //    return Ok(details);
        //}
    }
}
