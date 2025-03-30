using E_Learning.Data;
using E_Learning.Dto.Response.admin;
using E_Learning.Entity;

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
