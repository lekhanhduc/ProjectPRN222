using E_Learning.Data;
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

    }
}
