using E_Learning.Dto.Request;
using E_Learning.Dto.Response;
using E_Learning.Models.Response;
using E_Learning.Servies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.Controllers
{
    [Route("api/v1/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {

        private readonly ICommentService commentService;
        public CommentController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ApiResponse<CommentResponse>> AddComment([FromBody] CommentRequest request)
        {
            var result = await commentService.AddComment(request);
            return new ApiResponse<CommentResponse>
            {
                code = 201,
                message = "Success",
                result = result
            };
        }

        [HttpGet("{postId}")]
        public async Task<ApiResponse<PageResponse<CommentResponse>>> GetCommentByPostId(
                [FromRoute] long postId,
                [FromQuery] int page = 1,
                [FromQuery] int size = 4) 
        {
            var result = await commentService.GetCommentByPostId(postId, page, size);
            return new ApiResponse<PageResponse<CommentResponse>>
            {
                code = 200,
                message = "Success",
                result = result
            };
        }

        [HttpDelete("{commentId}")]
        [Authorize]
        public async Task<ApiResponse<string>> DeleteComment([FromRoute] long commentId)
        {
            await commentService.DeleteComment(commentId);
            return new ApiResponse<string>
            {
                code = 200,
                message = "Success",
                result = "Comment deleted successfully"
            };
        }

    }
}
