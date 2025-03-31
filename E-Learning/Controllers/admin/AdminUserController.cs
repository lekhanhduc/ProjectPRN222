using Microsoft.AspNetCore.Mvc;
using E_Learning.Servies;
using E_Learning.Models.Response;
using E_Learning.Services.admin;
using E_Learning.Dto.Response.admin;
using E_Learning.Dto.Response;
using Microsoft.AspNetCore.Authorization;

namespace E_Learning.Controllers.admin
{
    [ApiController]
    [Route("api/v1/admin/users")]
    //[Authorize(Roles = "ADMIN")]
    public class AdminUserController : ControllerBase
    {
        private readonly IAdminUserService _userService;

        public AdminUserController(IAdminUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers(
                    int page = 1,
                    int size = 10,
                    string sortBy = "createdAt",
                    string sortDir = "DESC")
        {
            var result = await _userService.GetAllUsersAsync(page, size, sortBy, sortDir);
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchUsers(string keywords, int page = 1, int size = 10, string sortBy = "createdAt", string sortDir = "DESC")
        {
            var keywordArray = keywords.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var result = await _userService.SearchUsersByKeywordsAsync(keywordArray, page, size, sortBy, sortDir);
            return Ok(result);
        }


        [HttpPost("ban/{userId}")]
        public async Task<IActionResult> BanUser(long userId)
        {
            await _userService.BanUserAsync(userId);
            return Ok(new { message = "User banned successfully" });
        }

        [HttpPost("unban/{userId}")]
        public async Task<IActionResult> UnbanUser(long userId)
        {
            await _userService.UnbanUserAsync(userId);
            return Ok(new { message = "User unbanned successfully" });
        }

        [HttpPut("{userId}/role")]
        public async Task<IActionResult> UpdateRole(long userId, [FromQuery] string roleName)
        {
            await _userService.UpdateUserRoleAsync(userId, roleName);
            return Ok(new { message = "User role updated successfully" });
        }

        [HttpGet("{userId}/details")]
        public async Task<IActionResult> GetUserDetail(long userId)
        {
            var detail = await _userService.GetUserDetailAsync(userId);
            return Ok(detail);
        }
    }

}

