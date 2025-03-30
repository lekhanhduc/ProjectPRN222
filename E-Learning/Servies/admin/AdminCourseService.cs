using E_Learning.Data;
using E_Learning.Dto.Response;
using E_Learning.Dto.Response.Admin;
using E_Learning.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Servies.admin
{
    public class AdminCourseService
    {
        private readonly ELearningDbContext _context;

        public AdminCourseService(ELearningDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<AdminCourseResponse>> GetAllCourses(int page, int size, string sortBy = "title", string sortDir = "asc")
        {
            var query = _context.Courses.AsQueryable();

            // Kiểm tra thuộc tính sắp xếp và áp dụng sắp xếp tương ứng
            if (sortBy.ToLower() == "title")
            {
                query = sortDir.ToLower() == "desc"
                    ? query.OrderByDescending(c => c.Title)
                    : query.OrderBy(c => c.Title);
            }
            else if (sortBy.ToLower() == "authorname")
            {
                query = sortDir.ToLower() == "desc"
                    ? query.OrderByDescending(c => c.Author.Name)
                    : query.OrderBy(c => c.Author.Name);
            }
            else if (sortBy.ToLower() == "createdat")
            {
                query = sortDir.ToLower() == "desc"
                    ? query.OrderByDescending(c => c.CreatedAt)
                    : query.OrderBy(c => c.CreatedAt);
            }
            else if (sortBy.ToLower() == "updatedat")
            {
                query = sortDir.ToLower() == "desc"
                    ? query.OrderByDescending(c => c.UpdatedAt)
                    : query.OrderBy(c => c.UpdatedAt);
            }
            // Thêm các trường cần sắp xếp khác ở đây nếu cần

            var result = query.Select(c => new AdminCourseResponse
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                Enabled = c.Enabled,
                AuthorId = c.AuthorId,
                AuthorName = c.Author.Name,
                Language = c.Language,
                Level = c.Level.ToString(),
                Duration = c.Duration,
                Thumbnail = c.Thumbnail,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            }).AsQueryable();

            return await PagedResult<AdminCourseResponse>.CreateAsync(result, page, size);
        }


        public async Task BanCourse(long courseId)
        {
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
                throw new AppException("Course not found.");

            if (!course.Enabled)
                throw new AppException("Course is already banned.");

            course.Enabled = false;
            await _context.SaveChangesAsync();
        }

        public async Task UnbanCourse(long courseId)
        {
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
                throw new AppException("Course not found.");

            course.Enabled = true;
            await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<AdminCourseResponse>> GetBannedCourses(int page, int size)
        {
            var query = _context.Courses.Where(c => !c.Enabled);
            return await PagedResult<AdminCourseResponse>.CreateAsync(query.Select(c => new AdminCourseResponse
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                Enabled = c.Enabled,
                AuthorId = c.AuthorId,
                AuthorName = c.Author.Name,
                Language = c.Language,
                Level = c.Level.ToString(),
                Duration = c.Duration,
                Thumbnail = c.Thumbnail,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            }), page, size);
        }

        public async Task<AdminCourseResponse> GetCourseDetails(long id)
        {
            var course = await _context.Courses.Include(c => c.Author).FirstOrDefaultAsync(c => c.Id == id);
            if (course == null)
                throw new AppException("Course not found.");

            return new AdminCourseResponse
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                Enabled = course.Enabled,
                AuthorId = course.AuthorId,
                AuthorName = course.Author.Name,
                Language = course.Language,
                Level = course.Level.ToString(),
                Duration = course.Duration,
                Thumbnail = course.Thumbnail,
                CreatedAt = course.CreatedAt,
                UpdatedAt = course.UpdatedAt
            };
        }

        public async Task<PagedResult<AdminCourseResponse>> GetActiveCourses(int page, int size)
        {
            var query = _context.Courses.Where(c => c.Enabled); // Lọc các khóa học đang hoạt động
            return await PagedResult<AdminCourseResponse>.CreateAsync(query.Select(c => new AdminCourseResponse
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                Enabled = c.Enabled,
                AuthorId = c.AuthorId,
                AuthorName = c.Author.Name,
                Language = c.Language,
                Level = c.Level.ToString(),
                Duration = c.Duration,
                Thumbnail = c.Thumbnail,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            }), page, size);
        }

        public async Task<PagedResult<AdminCourseResponse>> SearchCoursesByKeywords(string[] keywords, int page, int size, string sortBy = "title", string sortDir = "asc")
        {
            var query = _context.Courses.AsQueryable();

            // Tạo một truy vấn tìm kiếm với các từ khóa
            foreach (var keyword in keywords)
            {
                query = query.Where(c => c.Title.Contains(keyword) || c.Description.Contains(keyword));
            }

            // Áp dụng sắp xếp
            if (sortDir.ToLower() == "desc")
            {
                // Sắp xếp giảm dần
                query = sortBy.ToLower() switch
                {
                    "title" => query.OrderByDescending(c => c.Title),
                    "createdat" => query.OrderByDescending(c => c.CreatedAt),
                    _ => query.OrderByDescending(c => c.Title), // mặc định là sắp xếp theo title
                };
            }
            else
            {
                // Sắp xếp tăng dần
                query = sortBy.ToLower() switch
                {
                    "title" => query.OrderBy(c => c.Title),
                    "createdat" => query.OrderBy(c => c.CreatedAt),
                    _ => query.OrderBy(c => c.Title), // mặc định là sắp xếp theo title
                };
            }

            return await PagedResult<AdminCourseResponse>.CreateAsync(query.Select(c => new AdminCourseResponse
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                Enabled = c.Enabled,
                AuthorId = c.AuthorId,
                AuthorName = c.Author.Name,
                Language = c.Language,
                Level = c.Level.ToString(),
                Duration = c.Duration,
                Thumbnail = c.Thumbnail,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            }), page, size);
        }



    }
}
