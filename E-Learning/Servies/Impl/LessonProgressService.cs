using E_Learning.Dto.Request;
using E_Learning.Dto.Response;
using E_Learning.Entity;
using E_Learning.Repositories;
using E_Learning.Utils;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Numerics;
using E_Learning.Middlewares;

namespace E_Learning.Servies.Impl
{
    public class LessonProgressService : ILessonProgressService
    {
        private readonly LessonProgressRepository lessonProgressRepository;
        private readonly UserRepository userRepository;
        private readonly CourseRepository courseRepository;
        private readonly EnrollmentRepository enrollmentRepository;
        private readonly LessonRepository lessonRepository;
        private readonly IHttpContextAccessor httpContextAccessor;

        public LessonProgressService(
            LessonProgressRepository lessonProgressRepository,
            UserRepository userRepository,
            CourseRepository courseRepository,
            EnrollmentRepository enrollmentRepository,
            LessonRepository lessonRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            this.lessonProgressRepository = lessonProgressRepository;
            this.userRepository = userRepository;
            this.courseRepository = courseRepository;
            this.enrollmentRepository = enrollmentRepository;
            this.lessonRepository = lessonRepository;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserCompletionResponse> CalculateCompletion(long courseId)
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

            if (!await enrollmentRepository.ExistsByUserAndCourse(user, course))
            {
                throw new AppException(ErrorCode.COURSE_ACCESS_DENIED);
            }

            var totalLessons = course.Chapters
                .Sum(chapter => chapter.Lessons.Count);

            var completedLessons = await lessonProgressRepository.CountByUserAndCourseAndCompleted(user, course, true);

            if (totalLessons == 0)
            {
                return new UserCompletionResponse
                {
                    TotalLessonComplete = 0,
                    TotalLessons = 0,
                    CompletionPercentage = 0,
                    LessonCompletes = new List<UserCompletionResponse.LessonComplete>()
                };
            }

            var lessonProgresses = await lessonProgressRepository.FindByUserAndCourse(user, course, true);
            var listCompletedLessons = lessonProgresses.Select(lessonPro => new UserCompletionResponse.LessonComplete
            {
                LessonId = lessonPro.Lesson.Id,
                LessonName = lessonPro.Lesson.LessonName
            }).ToList();

            var completed = (decimal)completedLessons;
            var total = (decimal)totalLessons;
            var percentage = Math.Round((completed / total) * 100, 0);

            return new UserCompletionResponse
            {
                TotalLessonComplete = completedLessons,
                TotalLessons = totalLessons,
                LessonCompletes = listCompletedLessons,
                CompletionPercentage = percentage
            };
        }

        public async Task<LessonProgressResponse> MarkLessonAsCompleted(LessonProgressRequest request)
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

            var lesson = await lessonRepository.FindById(request.LessonId);
            if (lesson == null)
            {
                throw new AppException(ErrorCode.LESSON_NOT_EXIST);
            }

            var course = lesson.Chapter.Course;

            if (!await enrollmentRepository.ExistsByUserAndCourse(user, course))
            {
                throw new AppException(ErrorCode.COURSE_ACCESS_DENIED);
            }

            var existingProgress = await lessonProgressRepository.FindByUserAndLesson(user, lesson);
            if (existingProgress != null)
            {
                return new LessonProgressResponse
                {
                    LessonId = existingProgress.Lesson.Id,
                    LessonName = existingProgress.Lesson.LessonName,
                    IsComplete = existingProgress.Completed
                };
            }

            var newProgress = new LessonProgress
            {
                User = user,
                Lesson = lesson,
                Completed = true
            };
            await lessonProgressRepository.AddLessonProgress(newProgress);

            return new LessonProgressResponse
            {
                LessonId = lesson.Id,
                LessonName = lesson.LessonName,
                IsComplete = true
            };

        }
    }
}
