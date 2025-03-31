using E_Learning.Data;
using E_Learning.Dto.Response.admin;
﻿using E_Learning.Common;
using E_Learning.Data;
using E_Learning.Entity;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Repositories
{
    public class PaymentRepository
    {

        private readonly ELearningDbContext _context;

        public PaymentRepository(ELearningDbContext context)
        {
            _context = context;
        }

        public async Task CreatePayment(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
        }

        public async Task<Payment> FindByOrderCode(int orderCode)
        {
            return await _context.Payments.FirstOrDefaultAsync(payment => payment.OrderCode == orderCode);
        }

        public async Task<Payment> FindByOrderCode(long orderCode)
        {
            return await _context.Payments
                .Include(payment => payment.Course)
                .Include(payment => payment.User)
                .FirstOrDefaultAsync(payment => payment.OrderCode == orderCode);
        }

        public async Task UpdatePayment(Payment payment)
        {
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Payment>> FindByCourse(Course course)
        {
            return await _context.Payments
                .Where(p => p.Course.Id == course.Id)
                .ToListAsync();
        }
            public List<Payment> GetPaymentsByYear(int year)
        {
            return _context.Payments
                .Include(p => p.Course)
                    .ThenInclude(c => c.Author)
                        .ThenInclude(a => a.Role)
                .Where(p => p.CreatedAt.Year == year)
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

        public async Task<List<Payment>> FindPaymentsByAuthorStatusAndDateRangeAsync(User user, PaymentStatus status, DateTime start, DateTime end)
        {
            start = start.Date; 
            end = end.Date.AddDays(1).AddSeconds(-1); // Lấy đến cuối ngày (23:59:59) của ngày cuối cùng

            var payments = await _context.Payments
                .Include(p => p.Course)  
                .Where(p => p.Course.AuthorId == user.Id  
                            && p.Status == status
                            && p.CreatedAt >= start 
                            && p.CreatedAt <= end)  
                .ToListAsync();

            return payments;
        }

    }
}
