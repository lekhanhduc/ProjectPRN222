using E_Learning.Data;
using E_Learning.Entity;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Repositories
{
    public class CertificateRepository
    {

        private readonly ELearningDbContext _context;

        public CertificateRepository(ELearningDbContext context)
        {
            _context = context;
        }

        public async Task Add(Certificate certificate)
        {
            await _context.Certificates.AddAsync(certificate);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByCourseAndUser(Course course, User user)
        {
            return await _context.Certificates
                .AnyAsync(c => c.CourseId == course.Id && c.UserId == user.Id);
        }

        public async Task<List<Certificate>> FindByUserAsync(User user)
        {
            return await _context.Certificates
                .Where(c => c.UserId == user.Id)
                .Include(c => c.Course)         
                    .ThenInclude(course => course.Author) 
                .Include(c => c.User)         
                .ToListAsync();
        }

    }
}
