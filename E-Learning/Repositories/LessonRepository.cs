using E_Learning.Data;
using E_Learning.Entity;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Repositories
{
    public class LessonRepository
    {
        private readonly ELearningDbContext context;
        public LessonRepository(ELearningDbContext context)
        {
            this.context = context;
        }
        public async Task<Lesson> FindById(long id)
        {
            return await context.Lessons
                .Include(lesson => lesson.Chapter)                  // Lấy luôn Chapter của Lesson
                .ThenInclude(chapter => chapter.Course)              // Lấy luôn Course của Chapter
                .Include(lesson => lesson.LessonProgresses)          // Lấy luôn LessonProgresses liên quan đến Lesson
                .FirstOrDefaultAsync(c => c.Id == id);               // Lọc theo LessonId
        }

        public async Task<Lesson> Create(Lesson lesson)
        {
            await context.Lessons.AddAsync(lesson);
            await context.SaveChangesAsync();
            return lesson;
        }

        public async Task<Lesson> Update(Lesson lesson)
        {
            context.Lessons.Update(lesson);
            await context.SaveChangesAsync();
            return lesson;
        }

        public async Task Delete(Lesson lesson)
        {
            context.Lessons.Remove(lesson);
            await context.SaveChangesAsync();
        }

        public async Task<ICollection<Lesson>> FindAll()
        {
            return await context.Lessons.ToListAsync();
        }

    }
}
