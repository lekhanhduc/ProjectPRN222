using E_Learning.Dto.Request;
using E_Learning.Dto.Response;
using E_Learning.Entity;
using E_Learning.Middlewares;
using E_Learning.Repositories;
using Microsoft.AspNetCore.Http;

namespace E_Learning.Servies.Impl
{
    public class ChapterService: IChapterService
    {
        private readonly ChapterRepository chapterRepository;
        private readonly UserRepository userRepository;
        private readonly CourseRepository courseRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        public ChapterService(ChapterRepository chapterRepository, UserRepository userRepository, CourseRepository courseRepository, IHttpContextAccessor httpContextAccessor)
        {
            this.chapterRepository = chapterRepository;
            this.userRepository = userRepository;
            this.courseRepository = courseRepository;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<ChapterCreationResponse> CreateChapter(ChapterCreationRequest request)
        {
            var userId = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userId == null)
            {
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }

            var user = await userRepository.FindUserById(int.Parse(userId.Value));

            var course = await courseRepository.FindById(request.CourseId);
            if (course == null)
            {
                throw new AppException(ErrorCode.COURSE_NOT_EXISTED);
            }

            var chapter = new Chapter
            {
                ChapterName = request.ChapterName,
                Description = request.Description,
                Lessons = new List<Lesson>() 
            };



            chapter.Course = course;
            await chapterRepository.Create(chapter); 

            var response = new ChapterCreationResponse
            {
                UserName = user.Name,
                CourseId = course.Id,
                ChapterId = chapter.Id,
                ChapterName = chapter.ChapterName,
                Description = chapter.Description,
                Lessons = chapter.Lessons?.Select(lesson => new LessonDtoCreateChapter
                {
                    LessonId = lesson.Id,
                    LessonName = lesson.LessonName,
                    LessonDescription = lesson.Description,
                    VideoUrl = lesson.VideoUrl
                }).ToHashSet() ?? new HashSet<LessonDtoCreateChapter>()
            };

            return response;
        }


    }
}
