using E_Learning.Models.Request;
using E_Learning.Models.Response;
using E_Learning.Repositories;

namespace E_Learning.Servies.Impl
{
    public class CourseService : ICourseService
    {

        private readonly CourseRepository courseRepository;

        public CourseService(CourseRepository courseRepository)
        {
            this.courseRepository = courseRepository;
        }
        public Task<CreationCourseResponse> CreateCourse(CreationCourseRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CourseResponse>> FindAll()
        {
            var courses = await courseRepository.FindAll();

            var result = courses.Select(course => new CourseResponse
            {
                Title = course.Title,
                Author = course.Author.Name,
                Description = course.Description,
                Duration = course.Duration,
                Language = course.Language,
                Level = course.Level,
                Point = course.Point,
            });

            return result;
        }
    }
}
