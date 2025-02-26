using E_Learning.Models.Response;
using E_Learning.Servies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {

        private readonly ICourseService courseService;

        public CoursesController(ICourseService courseService)
        {
            this.courseService = courseService;
        }

        [Authorize]
        [HttpGet("fetch-all")]
        public async Task<ResponseData<IEnumerable<CourseResponse>>> FetchAll()
        {
            var courses = await courseService.FindAll();

            return new ResponseData<IEnumerable<CourseResponse>>(
                code: 200,
                message: "Fetch All Courses",
                data: courses);
        }

    }
}
