using E_Learning.Dto.Response.admin;
using E_Learning.Servies.admin;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.Controllers.admin
{
    [Route("api/v1/admin/reviews")]
    [ApiController]
    public class AdminReviewController : ControllerBase
    {
        private readonly IAdminReviewService _reviewService;

        public AdminReviewController(IAdminReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet("courses/rating")]
        public async Task<ActionResult<List<AdminReviewDTO>>> GetReviewsByCourseAndRating(
            [FromQuery] int month,
            [FromQuery] int year,
            [FromQuery] bool ascending = false)
        {
            var result = await _reviewService.GetReviewsByCourseAndRatingAsync(month, year, ascending);
            return Ok(result);
        }
    }


}
