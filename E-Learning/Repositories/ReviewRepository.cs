using E_Learning.Data;
using E_Learning.Entity;
using Microsoft.EntityFrameworkCore;
using E_Learning.Dto.Response.admin;
using Microsoft.EntityFrameworkCore;
using System;

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
        public async Task<List<AdminReviewDTO>> FindAverageRatingForCoursesInMonthYearAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Reviews
                .Include(r => r.Course)
                    .ThenInclude(c => c.Author)
                .Where(r => r.CreatedAt >= startDate && r.CreatedAt <= endDate)
                .GroupBy(r => new
                {
                    CourseId = r.Course.Id,
                    AuthorId = r.Course.Author.Id,
                    AuthorName = r.Course.Author.Name,
                    AuthorAvatar = r.Course.Author.Avatar,
                    Title = r.Course.Title,
                    Thumbnail = r.Course.Thumbnail
                })
                .Select(g => new AdminReviewDTO
                {
                    CourseId = g.Key.CourseId,
                    AverageRating = (double)g.Average(r => r.Rating),
                    CreatedAt = g.Max(r => r.CreatedAt),
                    UpdatedAt = g.Max(r => r.UpdatedAt),
                    UserId = g.Key.AuthorId,
                    UserName = g.Key.AuthorName,
                    UserAvatar = g.Key.AuthorAvatar,
                    Title = g.Key.Title,
                    Thumbnail = g.Key.Thumbnail
                })
                .ToListAsync();
        }

        public async Task<List<Review>> FindByLessonIdAsync(long lessonId)
        {
            return await _context.Reviews
                .Where(r => r.LessonId == lessonId)
                .Include(r => r.User) 
                .Include(r => r.Replies) 
                .ToListAsync();
        }

    }
}

