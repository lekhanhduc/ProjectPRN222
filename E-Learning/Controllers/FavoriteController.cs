using E_Learning.Dto.Request;
using E_Learning.Dto.Response;
using E_Learning.Models.Response;
using E_Learning.Servies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.Controllers
{
    [Route("api/v1")]
    [ApiController]
    [Authorize]
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteService favoriteService;

        public FavoriteController(IFavoriteService favoriteService)
        {
            this.favoriteService = favoriteService;
        }

        [HttpGet("favorites")]
        public async Task<ApiResponse<PageResponse<FavoriteResponse>>> GetFavoriteCourses([FromQuery] int page = 1, [FromQuery] int size = 6)
        {
            var result = await favoriteService.GetFavoriteCourses(page, size);
            return new ApiResponse<PageResponse<FavoriteResponse>>
            {
                code = 200,
                message = "Success",
                result = result
            };
        }


        [HttpPost("favorites")]
        public async Task<ApiResponse<object>> CreateFavorite([FromBody] FavoriteRequest request)
        {
            await favoriteService.CreateFavorite(request);
            return new ApiResponse<object>
            {
                code = 201,
                message = "Favorite course added successfully",
                result = null
            };
        }

        [HttpDelete("favorites/{favoriteId}")]
        public async Task<ApiResponse<object>> RemoveFavorite([FromRoute] int favoriteId)
        {
            await favoriteService.RemoveFavorite(favoriteId);
            return new ApiResponse<object>
            {
                code = 200,
                message = "Favorite course removed successfully",
                result = null
            };
        }

    }
}
