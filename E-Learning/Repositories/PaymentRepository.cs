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

    }
}
