using E_Learning.Dto.Request;
using E_Learning.Dto.Response;
using E_Learning.Models.Response;
using E_Learning.Servies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.Controllers
{
    [Route("api/v1/lesson")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly ILessonService lessonService;

        public LessonController(ILessonService lessonService)
        {
            this.lessonService = lessonService;
        }

        [HttpPost]
        [Authorize(Roles = "TEACHER")]
        public async Task<ApiResponse<LessonCreationResponse>> CreateLesson([FromForm] LessonCreationRequest request)
        {
            var result = await lessonService.CreateLesson(request);
            return new ApiResponse<LessonCreationResponse>
            {
                code = 201,
                message = "Created lesson",
                result = result
            };
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse<object>> DeleteLesson(long id)
        {
            await lessonService.DeleteLesson(id);
            return new ApiResponse<object>
            {
                code = 200,
                message = "Deleted lesson",
            };
        }

        [HttpPost("comment")]
        [Authorize]
        public async Task<ApiResponse<CommentLessonResponse>> AddCommentLesson([FromBody] CommentLessonRequest request)
        {
            var result = await lessonService.AddCommentLesson(request);
            return new ApiResponse<CommentLessonResponse>
            {
                code = 201,
                message = "Created comment",
                result = result
            };
        }

        [HttpGet("comment/{lessonId}")]
        [Authorize]
        public async Task<ApiResponse<List<CommentLessonResponse>>> GetCommentByLesson(long lessonId)
        {
            var result = await lessonService.GetCommentByLesson(lessonId);
            return new ApiResponse<List<CommentLessonResponse>>
            {
                code = 200,
                message = "Get comment by lesson",
                result = result
            };
        }

    }
}
