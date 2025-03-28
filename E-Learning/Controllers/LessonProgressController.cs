using E_Learning.Dto.Request;
using E_Learning.Dto.Response;
using E_Learning.Models.Response;
using E_Learning.Servies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class LessonProgressController : ControllerBase
    {

        private readonly ILessonProgressService lessonProgressService;

        public LessonProgressController(ILessonProgressService lessonProgressService)
        {
            this.lessonProgressService = lessonProgressService;
        }

        [HttpGet("calculate-completion/{courseId}")]
        public async Task<ApiResponse<UserCompletionResponse>> CalculateCompletion(long courseId)
        {
            var result = await lessonProgressService.CalculateCompletion(courseId);

            return new ApiResponse<UserCompletionResponse>
            {
                code = 200,
                message = "Calculate completion",
                result = result
            };
        }

        [HttpPost("update-lesson-progress")]
        public async Task<ApiResponse<object>> UpdateLessonProgress([FromBody] LessonProgressRequest request)
        {
            var result = await lessonProgressService.MarkLessonAsCompleted(request);
            return new ApiResponse<object>
            {
                code = 200,
                message = "Updated lesson progress",
                result = result
            };
        }

    }
}
