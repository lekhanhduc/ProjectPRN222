using E_Learning.Dto.Request;
using E_Learning.Dto.Response;
using E_Learning.Entity;
using E_Learning.Middlewares;
using E_Learning.Models.Response;
using E_Learning.Repositories;

namespace E_Learning.Servies.Impl
{
    public class CourseService : ICourseService
    {

        private readonly CourseRepository courseRepository;
        private readonly CloudinaryService cloudinaryService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public CourseService(CourseRepository courseRepository, CloudinaryService cloudinaryService, IHttpContextAccessor httpContextAccessor)
        {
            this.courseRepository = courseRepository;
            this.cloudinaryService = cloudinaryService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<CourseCreationResponse> CreateCourse(CourseCreationRequest request)
        {
            if (request == null)
            {
                throw new AppException(ErrorCode.ACCOUNT_LOCKED);
            }
            var userId = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userId == null)
            {
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }

            Course course = new Course();
            course.Title = request.Title;
            course.Description = request.Description;
            course.Duration = request.Duration;
            course.Language = request.Language;
            course.Level = request.CourseLevel;
            course.Price = request.Price;
            course.AuthorId = int.Parse(userId.Value);
            course.CreatedAt = DateTime.UtcNow;

            using var stream = request.Thumbnail.OpenReadStream();
            var imageUrl = await cloudinaryService.UploadImageAsync(stream, request.Thumbnail.Name);
            course.Thumbnail = imageUrl;
            await courseRepository.CreateCourse(course);

            return new CourseCreationResponse
            {
                Id = course.Id,
                Author = course.Author.Name,
                Title = course.Title,
                Description = course.Description,
                Duration = course.Duration,
                Language = course.Language,
                CourseLevel = course.Level,
                Thumbnail = course.Thumbnail,
                Price = course.Price,

            };
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
                Price = course.Price,
                Thumbnail = course.Thumbnail
            });

            return result;
        }
    }
}
