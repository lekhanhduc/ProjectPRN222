using E_Learning.Dto.Request;
using E_Learning.Dto.Response;
using E_Learning.Models.Response;
using E_Learning.Servies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.Controllers
{
    [Route("api/v1/teacher")]
    [ApiController]
    public class RevenueController : ControllerBase
    {
        private readonly IRevenueService revenueService;
        public RevenueController(IRevenueService revenueService)
        {
            this.revenueService = revenueService;
        }

        [HttpPost("revenue-detail")]
        [Authorize(Roles = "TEACHER")]

        public async Task<ApiResponse<RevenueSummaryResponse>> RevenueTeacher([FromBody] PeriodTypeRequest request)
        {
            Console.WriteLine(request.ToString());
            var result = await revenueService.RevenueTeacher(request);
            return new ApiResponse<RevenueSummaryResponse>
            {
                code = 200,
                message = "Success",
                result = result
            };
        }

    }
}
