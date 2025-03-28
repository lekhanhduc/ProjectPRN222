using E_Learning.Data;
using E_Learning.Entity;
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

        public async Task CreateCourse(Course course)
        {
            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();

            // Load Author ngay sau khi save mà không cần query lại toàn bộ entity
            await _context.Entry(course).Reference(c => c.Author).LoadAsync();
        }


        public async Task<List<Course>> FindAll(int page, int size)
        {
            if (page < 1)
            {
                page = 1;
            }
            return await _context.Courses
                .Include(course => course.Author)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();
        }

        public async Task<int> CountAllCourses()
        {
            return await _context.Courses.CountAsync();
        }

        public async Task<Course> FindById(long id)
        {
            return await _context.Courses
                .Include(course => course.Author)
                .FirstOrDefaultAsync(course => course.Id == id);
        }

        public async Task<Course> FindByIdAndChapter(long id)
        {
            return await _context.Courses
                .Include(course => course.Author)
                .Include(course => course.Chapters)
                .ThenInclude(chapter => chapter.Lessons) 
                .FirstOrDefaultAsync(course => course.Id == id);
        }


        public async Task<List<Course>> GetCoursesByAuthor(int authorId, int skip, int take)
        {
            return await _context.Courses
                .Where(c => c.AuthorId == authorId)
                .OrderByDescending(c => c.CreatedAt)
                .Skip(skip)
                .Take(take)
                .Include(c => c.Author) 
                .ToListAsync();
        }

        public async Task<int> CountCoursesByAuthor(int authorId)
        {
            return await _context.Courses.CountAsync(c => c.AuthorId == authorId);
        }

        public async Task UpdateCourse(Course course)
        {
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
        }
    }
}
