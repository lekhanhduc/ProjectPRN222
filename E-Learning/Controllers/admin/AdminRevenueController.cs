using E_Learning.Servies.admin;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.Controllers.admin
{

    // AdminRevenueController.cs
    [Route("api/v1/admin/revenue")]
    [ApiController]
    public class AdminRevenueController : ControllerBase
    {
        private readonly AdminRevenueService _revenueService;

        public AdminRevenueController(AdminRevenueService revenueService)
        {
            _revenueService = revenueService;
        }

        [HttpGet("teacher-revenue/year")]
        public IActionResult GetTeacherRevenueByYear(int year, bool ascending = false)
        {
            var data = _revenueService.GetTeacherRevenueByYear(year, ascending);
            return Ok(data);
        }

        [HttpGet("teacher-revenue/month")]
        public IActionResult GetTeacherRevenueByMonth(int month, int year, bool ascending = false)
        {
            var data = _revenueService.GetTeacherRevenueByMonth(month, year, ascending);
            return Ok(data);
        }

        [HttpGet("user-buy/month")]
        public IActionResult GetUserBuyStatsByMonth(int month, int year, bool ascending = false)
        {
            var data = _revenueService.GetUserBuyStatsByMonth(month, year, ascending);
            return Ok(data);
        }

        [HttpGet("user-buy/year")]
        public IActionResult GetUserBuyStatsByYear(int year, bool ascending = false)
        {
            var data = _revenueService.GetUserBuyStatsByYear(year, ascending);
            return Ok(data);
        }

        [HttpGet("monthly")]
        public IActionResult GetMonthlyRevenue(int year)
        {
            var data = _revenueService.GetMonthlyRevenue(year);
            return Ok(data);
        }

    }
}
