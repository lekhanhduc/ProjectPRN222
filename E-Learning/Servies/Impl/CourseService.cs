using E_Learning.Dto.Request;
using E_Learning.Dto.Response;
using E_Learning.Entity;
using E_Learning.Middlewares;
using E_Learning.Models.Response;
using E_Learning.Repositories;
using Elastic.Clients.Elasticsearch;

namespace E_Learning.Servies.Impl
{
    public class CourseService : ICourseService
    {

        private readonly CourseRepository courseRepository;
        private readonly CloudinaryService cloudinaryService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ElasticsearchClient elasticsearchClient;
        private readonly SearchRepository searchRepository;

        public CourseService(CourseRepository courseRepository, CloudinaryService cloudinaryService, IHttpContextAccessor httpContextAccessor, 
            ElasticsearchClient elasticsearchClient, SearchRepository searchRepository)
        {
            this.courseRepository = courseRepository;
            this.cloudinaryService = cloudinaryService;
            this.httpContextAccessor = httpContextAccessor;
            this.elasticsearchClient = elasticsearchClient;
            this.searchRepository = searchRepository;
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

            // Đồng bộ dữ liệu lên Elasticsearch
            var courseElastic = new CourseElasticSearch
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                Thumbnail = course.Thumbnail,
                Duration = course.Duration,
                Level = course.Level,
                Language = course.Language,
                Price = (double)course.Price,
                AuthorName = course.Author?.Name 
            };
            var indexResponse = await elasticsearchClient.IndexAsync(courseElastic, idx => idx
                .Index("courses") 
                .Id(course.Id.ToString()) 
            );

            if (!indexResponse.IsValidResponse)
            {
                Console.WriteLine($"Failed to index course in Elasticsearch: {indexResponse.DebugInformation}");
                throw new Exception($"Failed to index course in Elasticsearch: {indexResponse.DebugInformation}");
            } else
            {
                Console.WriteLine($"Indexed course {course.Id} in Elasticsearch");
            }
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

        public async Task<PageResponse<CourseResponse>> GetAllWithSearchElastic(int page, int size, string? keyword, string? level, double? minPrice, double? maxPrice)
        {
            return await searchRepository.GetAllWithSearch(page, size, keyword, level, minPrice, maxPrice);
        }
    }
}
