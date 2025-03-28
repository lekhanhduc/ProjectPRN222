using E_Learning.Data;
using E_Learning.Entity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Learning.Repositories
{
    public class LessonProgressRepository
    {
        private readonly ELearningDbContext context;

        public LessonProgressRepository(ELearningDbContext context)
        {
            this.context = context;
        }

        public async Task AddLessonProgress(LessonProgress lessonProgress)
        {
            await context.LessonProgresses.AddAsync(lessonProgress);
            await context.SaveChangesAsync();
        }

        public async Task<long> CountByUserAndCourseAndCompleted(User user, Course course, bool completed)
        {
            return await context.LessonProgresses
                .Where(lp => lp.User == user && lp.Lesson.Chapter.Course == course && lp.Completed == completed)
                .CountAsync();
        }

        public async Task<List<LessonProgress>> FindByUserAndCourse(User user, Course course)
        {
            return await context.LessonProgresses
                .Where(lp => lp.User == user && lp.Lesson.Chapter.Course == course)
                .ToListAsync();
        }

        public async Task<long> TotalLessonComplete(User user, Course course)
        {
            return await context.LessonProgresses
                .Where(lp => lp.User == user && lp.Lesson.Chapter.Course == course)
                .CountAsync();
        }

        public async Task<List<LessonProgress>> FindByLessonId(long lessonId)
        {
            return await context.LessonProgresses
                .Where(lp => lp.Lesson.Id == lessonId)
                .ToListAsync();
        }

        // Tìm tiến độ bài học của người dùng cho khóa học
        public async Task<List<LessonProgress>> FindByUserAndCourse(User user, Course course, bool completed)
        {
            return await context.LessonProgresses
                .Where(lp => lp.User == user && lp.Lesson.Chapter.Course == course && lp.Completed == completed)
                .ToListAsync();
        }

        // Kiểm tra tiến độ bài học của người dùng cho bài học cụ thể
        public async Task<LessonProgress?> FindByUserAndLesson(User user, Lesson lesson)
        {
            return await context.LessonProgresses
                .FirstOrDefaultAsync(lp => lp.User == user && lp.Lesson == lesson);
        }

    }

}
