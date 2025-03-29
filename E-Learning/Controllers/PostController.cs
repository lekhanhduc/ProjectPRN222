using E_Learning.Dto.Request;
using E_Learning.Dto.Response;
using E_Learning.Models.Response;
using E_Learning.Servies;
using Elastic.Clients.Elasticsearch;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.Controllers
{
    [Route("api/v1/post")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService postService;
        public PostController(IPostService postService)
        {
            this.postService = postService;
        }

        [HttpPost]
        public async Task<ApiResponse<PostCreationResponse>> CreatePost([FromForm] PostCreationRequest request)
        {
            var result = await postService.CreatePost(request);
            return new ApiResponse<PostCreationResponse>
            {
                code = 201,
                message = "Create post successfully",
                result = result
            };
        }

        [HttpGet]
        public async Task<ApiResponse<PageResponse<PostResponse>>> GetPost([FromQuery] int page = 1, [FromQuery] int size = 3)
        {
            var result = await postService.GetPost(page, size);
            return new ApiResponse<PageResponse<PostResponse>>
            {
                code = 200,
                message = "Get post successfully",
                result = result
            };
        }

    }
}
