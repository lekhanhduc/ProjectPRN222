using E_Learning.Dto.Request;
using E_Learning.Dto.Response;
using E_Learning.Models.Response;
using E_Learning.Servies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.Controllers
{
    [Route("api/v1/advertisement")]
    [ApiController]
    public class AdvertisementController : ControllerBase
    {
        private readonly IAdvertisementService advertisementService;
        public AdvertisementController(IAdvertisementService advertisementService)
        {
            this.advertisementService = advertisementService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ApiResponse<AdsCreationResponse>> CreateAdsAsync([FromForm] AdsCreationRequest request)
        {
            var result = await advertisementService.CreateAdsAsync(request);
            return new ApiResponse<AdsCreationResponse>
            {
                code = 201,
                message = "Advertisement created successfully",
                result = result
            };
        }

    }
}
