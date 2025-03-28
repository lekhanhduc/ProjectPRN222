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
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentService  enrollmentService;
        public EnrollmentController(IEnrollmentService enrollmentService)
        {
            this.enrollmentService = enrollmentService;
        }

        [HttpPost("courses/is-complete")]
        public async Task<ApiResponse<IsCourseCompleteResponse>> IsCourseCompleted([FromBody] IsCourseCompleteRequest request)
        {
            var result = await enrollmentService.IsCompleteCourse(request);
            return new ApiResponse<IsCourseCompleteResponse>
            {
                code = 200,
                message = "Check course completion",
                result = result
            };
        }

    }
}
