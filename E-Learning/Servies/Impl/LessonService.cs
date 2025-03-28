using E_Learning.Dto.Request;
using E_Learning.Dto.Response;
using E_Learning.Entity;
using E_Learning.Middlewares;
using E_Learning.Repositories;

namespace E_Learning.Servies.Impl
{
    public class LessonService : ILessonService
    {
        private readonly LessonRepository lessonRepository;
        private readonly CloudinaryService cloudinaryService;
        private readonly UserRepository userRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly CourseRepository courseRepository;
        private readonly ChapterRepository chapterRepository;

        public LessonService(LessonRepository lessonRepository, CloudinaryService cloudinaryService, IHttpContextAccessor httpContextAccessor, 
            UserRepository userRepository, CourseRepository courseRepository, ChapterRepository chapterRepository)
        {
            this.lessonRepository = lessonRepository;
            this.cloudinaryService = cloudinaryService;
            this.httpContextAccessor = httpContextAccessor;
            this.userRepository = userRepository;
            this.courseRepository = courseRepository;
            this.chapterRepository = chapterRepository;
        }

        public async Task<LessonCreationResponse> CreateLesson(LessonCreationRequest request)
        {
            var userId = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userId == null)
            {
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }
            var user = await userRepository.FindUserById(int.Parse(userId.Value));

            if (user == null)
            {
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }

            var course = await courseRepository.FindById(request.CourseId);
            if (course == null)
            {
                throw new AppException(ErrorCode.COURSE_NOT_EXISTED);
            }
            var chapter = await chapterRepository.FindById(request.ChapterId);
            if (chapter == null)
            {
                throw new AppException(ErrorCode.CHAPTER_NOT_EXIST);
            }
            string? videoUrl = null;
            if (request.Video != null)
            {
                using var stream = request.Video.OpenReadStream();
                videoUrl = await cloudinaryService.UploadVideoChunked(
                    stream,
                    request.Video.FileName, 
                    "upload"
                );
            }

            var lesson = new Lesson
            {
                Chapter = chapter,
                LessonName = request.LessonName,
                Description = request.Description,
                VideoUrl = videoUrl,
                ContentType = "VIDEO",
                CreatedAt = DateTime.UtcNow 
            };

            await lessonRepository.Create(lesson);

            return new LessonCreationResponse
            {
                CourseId = course.Id,
                ChapterId = chapter.Id,
                LessonId = lesson.Id,
                LessonName = lesson.LessonName,
                LessonDescription = lesson.Description,
                VideoUrl = lesson.VideoUrl
            };
        }

        public async Task DeleteLesson(long lessonId)
        {
            var lesson = await lessonRepository.FindById(lessonId);
            if (lesson == null)
            {
                throw new AppException(ErrorCode.LESSON_NOT_EXISTED);
            }
            await lessonRepository.Delete(lesson);
        }
    }
}
