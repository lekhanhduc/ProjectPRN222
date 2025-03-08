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

        public async Task<PageResponse<CourseResponse>> FindAll(int page, int size)
        {
            var courses = await courseRepository.FindAll(page, size);
            var totalElements = await courseRepository.CountAllCourses();
            var totalPages = (int)Math.Ceiling((double)totalElements / size);

            var result = courses.Select(course => new CourseResponse
            {
                Id = course.Id,
                Title = course.Title,
                Author = course.Author.Name,
                Description = course.Description,
                Duration = course.Duration,
                Language = course.Language,
                Level = course.Level,
                Price = course.Price,
                Thumbnail = course.Thumbnail
            }).ToList();

            return new PageResponse<CourseResponse>
            {
                CurrentPage = page,
                PageSize = size,
                TotalPages = totalPages,
                TotalElemets = totalElements,
                Data = result
            };
        }

        public async Task<CourseResponse> FindById(int id)
        {
            var course = await courseRepository.FindById(id);
            if (course == null)
            {
                throw new AppException(ErrorCode.COURSE_NOT_EXISTED);
            }

            return new CourseResponse
            {
                Id = course.Id,
                Title = course.Title,
                Author = course.Author.Name,
                Description = course.Description,
                Duration = course.Duration,
                Language = course.Language,
                Level = course.Level,
                Price = course.Price,
                Thumbnail = course.Thumbnail
            };

        }
    }
}
