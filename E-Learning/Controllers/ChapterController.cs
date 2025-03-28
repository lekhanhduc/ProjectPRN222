using E_Learning.Dto.Request;
using E_Learning.Dto.Response;
using E_Learning.Models.Response;
using E_Learning.Servies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.Controllers
{
    [Route("api/v1/chapter")]
    [ApiController]
    public class ChapterController : ControllerBase
    {
        private readonly IChapterService chapterService;

        public ChapterController(IChapterService chapterService)
        {
            this.chapterService = chapterService;
        }

        [HttpPost]
        public async Task<ApiResponse<ChapterCreationResponse>> CreateChapter([FromBody] ChapterCreationRequest request)
        {
            var result = await chapterService.CreateChapter(request);
            return new ApiResponse<ChapterCreationResponse>
            {
                code = 201,
                message = "Created chapter",
                result = result
            };
        }

    }
}
