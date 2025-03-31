using E_Learning.Dto.Request;
using E_Learning.Dto.Response;
using E_Learning.Dto.Response.Admin; // Đảm bảo import đúng namespace
using E_Learning.Models.Response;
using E_Learning.Servies.admin;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace E_Learning.Controllers.admin
{
    [Route("api/v1/admin/ads")]
    [ApiController]
    public class AdminAdvertisementController : ControllerBase
    {
        private readonly IAdminAdvertisementService _adminAdvertisementService;

        public AdminAdvertisementController(IAdminAdvertisementService adminAdvertisementService)
        {
            _adminAdvertisementService = adminAdvertisementService;
        }

        [HttpGet("fetch-ads")]
        public async Task<ActionResult<ApiResponse<PageResponse<AdsCreationResponse>>>> GetAllAds([FromQuery] int page = 1, [FromQuery] int size = 7)
        {
            var result = await _adminAdvertisementService.GetAllAds(page, size);
            return Ok(new ApiResponse<PageResponse<AdsCreationResponse>> { code = 200, result = result });
        }

        [HttpPut("approve-ads")]
        public async Task<ActionResult<ApiResponse<AdsApproveResponse>>> ApproveAds([FromBody] AdsApproveRequest request)
        {
            var response = await _adminAdvertisementService.ApproveAds(request);
            return Ok(new ApiResponse<AdsApproveResponse> { code = 200, result = response });
        }

        [HttpPut("reject-ads")]
        public async Task<ActionResult<ApiResponse<AdsApproveResponse>>> RejectAds([FromBody] AdsApproveRequest request)
        {
            var response = await _adminAdvertisementService.RejectAds(request);
            return Ok(new ApiResponse<AdsApproveResponse> { code = 200, result = response });
        }
    }
}
