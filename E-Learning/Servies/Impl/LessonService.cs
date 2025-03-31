using E_Learning.Dto.Request;
using E_Learning.Dto.Response;
using E_Learning.Entity;
using E_Learning.Middlewares;
using E_Learning.Repositories;
using Microsoft.AspNetCore.Authorization;

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
        private readonly ReviewRepository reviewRepository;

        public LessonService(LessonRepository lessonRepository, CloudinaryService cloudinaryService, IHttpContextAccessor httpContextAccessor, 
            UserRepository userRepository, CourseRepository courseRepository, ChapterRepository chapterRepository, ReviewRepository reviewRepository)
        {
            this.lessonRepository = lessonRepository;
            this.cloudinaryService = cloudinaryService;
            this.httpContextAccessor = httpContextAccessor;
            this.userRepository = userRepository;
            this.courseRepository = courseRepository;
            this.chapterRepository = chapterRepository;
            this.reviewRepository = reviewRepository;
        }

        public async Task<CommentLessonResponse> AddCommentLesson(CommentLessonRequest request)
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

            var lesson = await lessonRepository.FindById(request.LessonId)
                    ?? throw new AppException(ErrorCode.LESSON_NOT_EXIST);

            Review parentReview = null;
            if (request.ParentReviewId.HasValue)
            {
                parentReview = await reviewRepository.FindByIdAsync(request.ParentReviewId.Value)
                                ?? throw new AppException(ErrorCode.PARENT_COMMENT_NOT_EXISTED);
            }

            if (string.IsNullOrEmpty(request.Content))
            {
                throw new AppException(ErrorCode.INVALID_COMMENT_CONTENT);
            }

            var review = new Review
            {
                Course = course,
                Chapter = chapter,
                Lesson = lesson,
                User = user,
                Content = request.Content,
                ParentReview = parentReview
            };

            await reviewRepository.Add(review);

            var response = new CommentLessonResponse
            {
                ReviewId = review.Id,
                CourseId = course.Id,
                ChapterId = chapter.Id,
                LessonId = lesson.Id,
                Name = user.Name,  
                Avatar = user.Avatar, 
                Content = review.Content,
                CreatedAt = review.CreatedAt,
                UpdatedAt = review.UpdatedAt,
                Replies = review.Replies?.Select(r => new CommentLessonResponse
                {
                    ReviewId = r.Id,
                    CourseId = r.CourseId ?? 0,
                    ChapterId = r.ChapterId ?? 0,
                    LessonId = r.LessonId ?? 0,
                    Name = r.User.Name,
                    Avatar = r.User.Avatar,
                    Content = r.Content,
                    CreatedAt = r.CreatedAt,
                    UpdatedAt = r.UpdatedAt
                }).ToList() ?? new List<CommentLessonResponse>() 
            };

            return response;

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

        public async Task<List<CommentLessonResponse>> GetCommentByLesson(long lessonId)
        {
            var reviews = await reviewRepository.FindByLessonIdAsync(lessonId);

            var commentResponses = reviews.Select(review => new CommentLessonResponse
            {
                ReviewId = review.Id,
                CourseId = review.CourseId ?? 0,
                ChapterId = review.ChapterId ?? 0,
                LessonId = review.LessonId ?? 0,
                Name = review.User.Name, 
                Avatar = review.User.Avatar, 
                Content = review.Content,
                CreatedAt = review.CreatedAt,
                UpdatedAt = review.UpdatedAt,
                Replies = review.Replies?.Select(reply => new CommentLessonResponse
                {
                    ReviewId = reply.Id,
                    CourseId = reply.CourseId ?? 0,
                    ChapterId = reply.ChapterId ?? 0,
                    LessonId = reply.LessonId ?? 0,
                    Name = reply.User.Name,
                    Avatar = reply.User.Avatar,
                    Content = reply.Content,
                    CreatedAt = reply.CreatedAt,
                    UpdatedAt = reply.UpdatedAt
                }).ToList() ?? new List<CommentLessonResponse>() // Nếu không có replies, trả về danh sách rỗng
            }).ToList();

            return commentResponses;
        }

    }
}
