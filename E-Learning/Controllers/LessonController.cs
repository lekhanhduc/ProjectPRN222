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

    }
}
