using E_Learning.Data;
using E_Learning.Entity;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Repositories
{
    public class ReviewRepository
    {
        private readonly ELearningDbContext _context;

        public ReviewRepository(ELearningDbContext context)
        {
            _context = context;
        }

        public async Task Add(Review review)
        {
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Review review)
        {
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Review review)
        {
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
        }

        public async Task<Review> GetById(long id)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Review> FindByIdAsync(long id)
        {
            return await _context.Reviews
                .Include(r => r.Course)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<Review>> FindByCourseIdAndChapterIsNullAndLessonIsNullAsync(long courseId)
        {
            return await _context.Reviews
                .Where(r => r.CourseId == courseId && r.ChapterId == null && r.LessonId == null)
                .Include(r => r.Course)
                .Include(r => r.User) 
                .ToListAsync();
        }


    }
}
