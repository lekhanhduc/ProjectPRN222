    using E_Learning.Common;
    using E_Learning.Data;
    using E_Learning.Dto.Response;
    using E_Learning.Dto.Response.Admin;
    using E_Learning.Entity;
    using E_Learning.Middlewares;
    using E_Learning.Models.Response;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;

    namespace E_Learning.Services.admin
    {
        public class AdminTeacherService
        {
            private readonly ELearningDbContext _context;

            public AdminTeacherService(ELearningDbContext context)
            {
                _context = context;
            }

            // Get all teachers
            public async Task<PagedResult<AdminTeacherResponse>> GetAllTeachers(int page, int size, string sortBy = "name", string sortDir = "asc")
            {
                var query = _context.Users.AsQueryable()
                            .Where(u => u.Role.Name == "TEACHER");

                // Apply sorting
                query = ApplySorting(query, sortBy, sortDir);

                // Chuyển đổi User sang AdminTeacherResponse thủ công
                var teacherResponses = query.Select(u => new AdminTeacherResponse
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    Gender = u.Gender.ToString(),
                    Role = u.Role.Name,
                    CreatedAt = u.CreatedAt
                }).AsQueryable();

                // Trả về phân trang kết quả
                return await PagedResult<AdminTeacherResponse>.CreateAsync(teacherResponses, page, size);
            }

            // Search teachers by keywords
            public async Task<PagedResult<AdminTeacherResponse>> SearchTeachersByKeywords(string[] keywords, int page, int size, string sortBy = "name", string sortDir = "asc")
            {
                var query = _context.Users.AsQueryable()
                            .Where(u => u.Role.Name == "TEACHER" &&
                                        keywords.Any(k => u.Name.Contains(k) || u.Email.Contains(k)));

                // Apply sorting
                query = ApplySorting(query, sortBy, sortDir);

                // Chuyển đổi User sang AdminTeacherResponse thủ công
                var teacherResponses = query.Select(u => new AdminTeacherResponse
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    Gender = u.Gender.ToString(),
                    Role = u.Role.Name,
                    CreatedAt = u.CreatedAt
                }).AsQueryable();

                // Trả về phân trang kết quả
                return await PagedResult<AdminTeacherResponse>.CreateAsync(teacherResponses, page, size);
            }

            // Remove teacher role
            public async Task RemoveTeacherRole(long userId)
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                    throw new AppException(ErrorCode.USER_NOT_EXISTED);

                var userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "USER");
                if (userRole == null)
                    throw new AppException(ErrorCode.ROLE_NOT_EXISTED);

                user.Role = userRole;
                user.RegistrationStatus = null;

                await _context.SaveChangesAsync();
            }

            // Get pending teacher applications
            public async Task<PagedResult<AdminUserResponse>> GetPendingTeacherApplications(int page, int size, string sortBy = "name", string sortDir = "asc")
            {
                var query = _context.Users.AsQueryable()
                            .Where(u => u.Role.Name == "USER" && u.RegistrationStatus == RegistrationStatus.Pending);

                // Apply sorting
                query = ApplySorting(query, sortBy, sortDir);

                // Chuyển đổi User sang AdminUserResponse thủ công
                var userResponses = query.Select(u => new AdminUserResponse
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    Enabled = u.Enabled,
                    Gender = u.Gender.ToString(),
                    Role = u.Role.Name,
                    CreatedAt = u.CreatedAt
                }).AsQueryable();

                // Trả về phân trang kết quả
                return await PagedResult<AdminUserResponse>.CreateAsync(userResponses, page, size);
            }

        // Áp dụng sắp xếp
        private IQueryable<User> ApplySorting(IQueryable<User> query, string sortBy, string sortDir)
        {
            // Kiểm tra nếu sắp xếp theo "name"
            if (sortBy.Equals("name", StringComparison.OrdinalIgnoreCase))
            {
                query = sortDir.ToLower() == "desc" ? query.OrderByDescending(u => u.Name) : query.OrderBy(u => u.Name);
            }
            else if (sortBy.Equals("email", StringComparison.OrdinalIgnoreCase))
            {
                query = sortDir.ToLower() == "desc" ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email);
            }
            else if (sortBy.Equals("createdAt", StringComparison.OrdinalIgnoreCase))
            {
                query = sortDir.ToLower() == "desc" ? query.OrderByDescending(u => u.CreatedAt) : query.OrderBy(u => u.CreatedAt);
            }
            else
            {
                // Mặc định sắp xếp theo "name"
                query = sortDir.ToLower() == "desc" ? query.OrderByDescending(u => u.Name) : query.OrderBy(u => u.Name);
            }

            return query;
        }

    }
}

