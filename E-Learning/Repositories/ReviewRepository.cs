using E_Learning.Data;
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

    }
}