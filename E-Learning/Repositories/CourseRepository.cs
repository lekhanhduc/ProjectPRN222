using E_Learning.Data;
using E_Learning.Entity;
using E_Learning.Models.Request;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Repositories
{
    public class CourseRepository
    {

        private readonly ELearningDbContext _context;

        public CourseRepository(ELearningDbContext context)
        {
            _context = context;
        }

        public Task<Course> CreateCourse(CreationCourseRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Course>> FindAll()
        {
            return await _context.Courses
                .Include(course => course.Author)
                .ToListAsync();
        }

    }
}
