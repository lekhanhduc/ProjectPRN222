using E_Learning.Data;
using E_Learning.Dto.Response.admin;
using E_Learning.Entity;
using Microsoft.EntityFrameworkCore;
using System;

namespace E_Learning.Repositories
{
    public class AdminRevenueRepository
    {
        private readonly ELearningDbContext _context;

        public AdminRevenueRepository(ELearningDbContext context)
        {
            _context = context;
        }

        public List<Payment> GetPaymentsByYear(int year)
        {
            var start = new DateTime(year, 1, 1);
            var end = start.AddYears(1).AddTicks(-1);

            return _context.Payments
                .Include(p => p.Course)
                    .ThenInclude(c => c.Author)
                        .ThenInclude(a => a.Role)
                .Where(p => p.CreatedAt >= start && p.CreatedAt <= end)
                .ToList();
        }



        public List<Payment> GetPaymentsByMonth(int month, int year)
        {
            var start = new DateTime(year, month, 1);
            var end = start.AddMonths(1).AddTicks(-1);

            return _context.Payments
                .Include(p => p.Course)
                    .ThenInclude(c => c.Author)
                        .ThenInclude(a => a.Role)
                .Where(p => p.CreatedAt >= start && p.CreatedAt <= end)
                .ToList();
        }

        public List<AdminMonthlyRevenueDTO> GetMonthlyRevenue(int year)
        {
            return _context.Payments
                .Where(p => p.CreatedAt.Year == year)
                .GroupBy(p => p.CreatedAt.Month)
                .Select(g => new AdminMonthlyRevenueDTO
                {
                    Month = g.Key,
                    Year = year,
                    TotalRevenue = g.Sum(p => p.Price),
                    TotalCoursesSold = g.Count()
                })
                .OrderBy(r => r.Month)
                .ToList();
        }

    }
}
