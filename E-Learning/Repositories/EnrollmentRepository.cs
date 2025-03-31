using E_Learning.Data;
using E_Learning.Dto.Response.admin;
using E_Learning.Entity;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Repositories
{
    public class EnrollmentRepository
    {

        private readonly ELearningDbContext _context;

        public EnrollmentRepository(ELearningDbContext context)
        {
            _context = context;
        }

        public void Add(Enrollment enrollment)
        {
            _context.Enrollments.Add(enrollment);
            _context.SaveChanges();
        }

        public void Remove(Enrollment enrollment)
        {
            _context.Enrollments.Remove(enrollment);
            _context.SaveChanges();
        }

        public async Task<Enrollment?> CheckPurchase(long userId, long courseId)
        {
            return await _context.Enrollments
                .FirstOrDefaultAsync(e => e.UserId == userId && e.CourseId == courseId);
        }

        // Kiểm tra xem người dùng đã đăng ký khóa học chưa
        public async Task<bool> ExistsByUserAndCourse(User user, Course course)
        {
            return await _context.Enrollments
                .AnyAsync(e => e.UserId == user.Id && e.CourseId == course.Id);
        }

        public async Task<bool> IsCourseCompleteByUser(User user, Course course)
        {
            return await _context.Enrollments
                .Where(e => e.UserId == user.Id && e.CourseId == course.Id)
                .Select(e => e.IsComplete)
                .FirstOrDefaultAsync();
        }

        public async Task Update(Enrollment enrollment)
        {
            _context.Enrollments.Update(enrollment);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Enrollment>> FindCourseByUser(User user)
        {
            return await _context.Enrollments
                .Where(enrollment => enrollment.UserId == user.Id)
                .Include(enrollment => enrollment.Course)  
                .ThenInclude(course => course.Author)    
                .ToListAsync();
        }


        public List<AdminUserBuyDTO> GetUserBuyStatsByMonth(int month, int year)
        {
            var start = new DateTime(year, month, 1);
            var end = start.AddMonths(1).AddTicks(-1);

            return _context.Enrollments
                .Where(e => e.Purchased && e.CreatedAt >= start && e.CreatedAt <= end)
                .GroupBy(e => e.User)
                .Select(g => new AdminUserBuyDTO
                {
                    UserId = g.Key.Id,
                    FullName = g.Key.FullName,
                    TotalCoursesBought = g.Count()
                })
                .ToList();
        }

        public List<AdminUserBuyDTO> GetUserBuyStatsByYear(int year)
        {
            var start = new DateTime(year, 1, 1);
            var end = start.AddYears(1).AddTicks(-1);

            return _context.Enrollments
                .Where(e => e.Purchased && e.CreatedAt >= start && e.CreatedAt <= end)
                .GroupBy(e => e.User)
                .Select(g => new AdminUserBuyDTO
                {
                    UserId = g.Key.Id,
                    FullName = g.Key.FullName,
                    TotalCoursesBought = g.Count()
                })
                .ToList();
        }
    }
}
