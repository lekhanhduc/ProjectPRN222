using E_Learning.Common;
using E_Learning.Dto.Request;
using E_Learning.Dto.Response;
using E_Learning.Dto.Response.E_Learning.Dto.Response;
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
        private readonly UserRepository userRepository;
        private readonly CloudinaryService cloudinaryService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ElasticsearchClient elasticsearchClient;
        private readonly SearchRepository searchRepository;
        private readonly PaymentRepository paymentRepository;
        private readonly EnrollmentRepository enrollmentRepository;

        public CourseService(CourseRepository courseRepository, CloudinaryService cloudinaryService, IHttpContextAccessor httpContextAccessor, 
            ElasticsearchClient elasticsearchClient, SearchRepository searchRepository, PaymentRepository paymentRepository, UserRepository userRepository,
            EnrollmentRepository enrollmentRepository)
        {
            this.courseRepository = courseRepository;
            this.cloudinaryService = cloudinaryService;
            this.httpContextAccessor = httpContextAccessor;
            this.elasticsearchClient = elasticsearchClient;
            this.searchRepository = searchRepository;
            this.paymentRepository = paymentRepository;
            this.userRepository = userRepository;
            this.enrollmentRepository = enrollmentRepository;
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
            course.Quantity = 0;
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

        public async Task<PageResponse<CourseResponse>> GetCourseByTeacher(int page, int size)
        {
            var userIdClaim = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userIdClaim == null)
            {
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }
            int userId = int.Parse(userIdClaim.Value);
            int skip = (page - 1) * size;

            var courses = await courseRepository.GetCoursesByAuthor(userId, skip, size);
            var total = await courseRepository.CountCoursesByAuthor(userId);

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
                TotalPages = (int)Math.Ceiling((double)total / size),
                TotalElemets = total,
                Data = result
            };

        }

        public async Task<OverviewCourseResponse> OverviewCourseDetail(long id)
        {
            var course = await courseRepository.FindById(id);
            if (course == null)
            {
                throw new AppException(ErrorCode.COURSE_NOT_EXISTED);
            }

            var reviews = course.Reviews?.Where(r => r.Rating > 0).ToList() ?? new List<Review>();

            long totalRating = reviews.Sum(r => (long)r.Rating);
            int numberOfReviews = reviews.Count;

            decimal avgRating = numberOfReviews > 0
                ? (decimal)totalRating / numberOfReviews
                : 0;

            var payments = await paymentRepository.FindByCourse(course);
            decimal totalPrice = payments?.Sum(p => p.Price) ?? 0;

            return new OverviewCourseResponse
            {
                Quantity = course.Quantity,
                AvgReview = avgRating,
                TotalPrice = totalPrice
            };
        }

        public async Task<CourseChapterResponse> GetAllInfoCourse(long courseId)
        {
            var course = await courseRepository.FindByIdAndChapter(courseId);
            if (course == null)
            {
                throw new AppException(ErrorCode.COURSE_NOT_EXISTED);
            }

            long totalLessons = course.Chapters?
                .Sum(ch => ch.Lessons?.Count ?? 0) ?? 0;

            var chapterDtos = course.Chapters?
                .OrderBy(ch => ch.Id)
                .Select(chapter => new CourseChapterResponse.ChapterDto
                {
                    ChapterId = chapter.Id,
                    ChapterName = chapter.ChapterName,
                    LessonDto = chapter.Lessons?
                        .OrderBy(ls => ls.Id)
                        .Select(lesson => new CourseChapterResponse.LessonDto
                        {
                            LessonId = lesson.Id,
                            LessonName = lesson.LessonName,
                            Description = lesson.Description,
                            VideoUrl = lesson.VideoUrl
                        }).ToHashSet() ?? new HashSet<CourseChapterResponse.LessonDto>()
                }).ToHashSet() ?? new HashSet<CourseChapterResponse.ChapterDto>();

            return new CourseChapterResponse
            {
                CourseId = course.Id,
                CourseTitle = course.Title,
                CourseDescription = course.Description,
                TotalLesson = totalLessons,
                Chapters = chapterDtos
            };
        }

        public async Task<CoursePurchaseResponse> CheckPurchase(long courseId)
        {

            var userIdClaim = httpContextAccessor.HttpContext?.User?.Claims
                .FirstOrDefault(c => c.Type == "userId");

            if (userIdClaim == null)
            {
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }

            var userId = long.Parse(userIdClaim.Value);

            var user = await userRepository.FindUserById(userId);

            if (user == null)
            {
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }

            var course = await courseRepository.FindById(courseId);
            if (course == null)
            {
                throw new AppException(ErrorCode.COURSE_NOT_EXISTED);
            }

            if (user.Role?.Name == DefinitionRole.ADMIN)
            {
                return new CoursePurchaseResponse
                {
                    UserId = user.Id,
                    CourseId = course.Id,
                    Purchased = true
                };
            }

            var enrollment = await enrollmentRepository.CheckPurchase(user.Id, course.Id);
            if (enrollment == null)
            {
                return new CoursePurchaseResponse
                {
                    UserId = user.Id,
                    CourseId = course.Id,
                    Purchased = false
                };
            }

            return new CoursePurchaseResponse
            {
                UserId = user.Id,
                CourseId = course.Id,
                Purchased = enrollment.Purchased
            };
        }

        public async Task<CourseChapterResponse> InfoCourse(long courseId)
        {
            var course = await courseRepository.FindByIdAndChapter(courseId);
            if (course == null)
            {
                throw new AppException(ErrorCode.COURSE_NOT_EXISTED);
            }

            var totalLessons = course.Chapters
                .Sum(chapter => chapter.Lessons.Count);

            var courseLessonResponse = new CourseChapterResponse
            {
                CourseId = courseId,
                CourseTitle = course.Title,
                CourseDescription = course.Description,
                TotalLesson = totalLessons,
                Chapters = new HashSet<CourseChapterResponse.ChapterDto>()
            };

            foreach (var chapter in course.Chapters.OrderBy(c => c.Id))
            {
                var chapterDto = new CourseChapterResponse.ChapterDto
                {
                    ChapterId = chapter.Id,
                    ChapterName = chapter.ChapterName,
                    LessonDto = new HashSet<CourseChapterResponse.LessonDto>()
                };

                foreach (var lesson in chapter.Lessons.OrderBy(l => l.Id))
                {
                    var lessonDto = new CourseChapterResponse.LessonDto
                    {
                        LessonId = lesson.Id,
                        LessonName = lesson.LessonName,
                        Description = lesson.Description,
                        VideoUrl = lesson.VideoUrl
                    };

                    chapterDto.LessonDto.Add(lessonDto);
                }

                courseLessonResponse.Chapters.Add(chapterDto);
            }

            return courseLessonResponse;
        }
    }
}
