using E_Learning.Common;
using E_Learning.Data;
using E_Learning.Dto.Response;
using E_Learning.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Services.admin
{
    public class AdminTeacherService : IAdminTeacherService
    {
        private readonly ELearningDbContext _context;

        public AdminTeacherService(ELearningDbContext context)
        {
            _context = context;
        }

        public PagedResult<UserResponse> GetAllTeachers(int page, int size, string sortBy, bool isDescending)
        {
            var teacherRole = "TEACHER";

            var query = _context.Users.Include(u => u.Role)
                .Where(u => u.Role.Name == teacherRole);

            query = isDescending
                ? query.OrderByDescending(u => EF.Property<object>(u, sortBy))
                : query.OrderBy(u => EF.Property<object>(u, sortBy));

            var total = query.Count();

            var teachers = query.Skip((page - 1) * size).Take(size)
                .Select(u => new UserResponse
                {
                    Email = u.Email,
                    Name = u.Name,
                    FirstName = u.FirstName,
                    LastName = u.LastName
                }).ToList();

            return new PagedResult<UserResponse>(teachers, total, page, size);
        }

        public PagedResult<UserResponse> SearchTeachers(string keywords, int page, int size, string sortBy, bool isDescending)
        {
            var teacherRole = "TEACHER";
            var keywordArray = keywords.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var query = _context.Users.Include(u => u.Role)
                .Where(u => u.Role.Name == teacherRole);

            // Xây dựng điều kiện tìm kiếm bằng Expression hợp lệ với EF Core
            if (keywordArray.Length > 0)
            {
                query = query.Where(u => keywordArray
                    .Any(kw => u.Name.Contains(kw)) || keywordArray.Any(kw => u.Email.Contains(kw)));
            }

            // Xử lý sorting an toàn, rõ ràng
            query = sortBy?.ToLower() switch
            {
                "email" => isDescending ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email),
                "firstname" => isDescending ? query.OrderByDescending(u => u.FirstName) : query.OrderBy(u => u.FirstName),
                "lastname" => isDescending ? query.OrderByDescending(u => u.LastName) : query.OrderBy(u => u.LastName),
                _ => isDescending ? query.OrderByDescending(u => u.Name) : query.OrderBy(u => u.Name),
            };

            // Lấy tổng số record trước khi phân trang
            var total = query.Count();

            // Phân trang
            var teachers = query.Skip((page - 1) * size).Take(size)
                .Select(u => new UserResponse
                {
                    Email = u.Email,
                    Name = u.Name,
                    FirstName = u.FirstName,
                    LastName = u.LastName
                }).ToList();

            return new PagedResult<UserResponse>(teachers, total, page, size);
        }



        public void RemoveTeacherRole(long userId)
        {
            var user = _context.Users.Include(u => u.Role).FirstOrDefault(u => u.Id == userId);
            var userRole = _context.Roles.FirstOrDefault(r => r.Name == "USER");

            if (user == null || userRole == null)
                throw new Exception("User or role not found");

            user.RoleId = userRole.Id;
            _context.SaveChanges();
        }

        public PagedResult<UserResponse> GetPendingTeacherApplications(int page, int size, string sortBy, bool isDescending)
        {
            var pendingStatus = RegistrationStatus.Pending; // enum thay vì string
            var userRole = "USER";

            var query = _context.Users.Include(u => u.Role)
                .Where(u => u.Role.Name == userRole && u.RegistrationStatus == pendingStatus);

            query = isDescending
                ? query.OrderByDescending(u => EF.Property<object>(u, sortBy))
                : query.OrderBy(u => EF.Property<object>(u, sortBy));

            var total = query.Count();

            var applications = query.Skip((page - 1) * size).Take(size)
                .Select(u => new UserResponse
                {
                    Email = u.Email,
                    Name = u.Name,
                    FirstName = u.FirstName,
                    LastName = u.LastName
                }).ToList();

            return new PagedResult<UserResponse>(applications, total, page, size);
        }
    }
}

