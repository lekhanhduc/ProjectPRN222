using E_Learning.Data;
using E_Learning.Dto.Response;
using E_Learning.Dto.Response.admin;
using E_Learning.Entity;
using E_Learning.Models.Response;
using E_Learning.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PagedList;

namespace E_Learning.Services.admin
{
    public class AdminUserService : IAdminUserService
    {
        private readonly ELearningDbContext _context;
        private readonly UserRepository _userRepository;

        public AdminUserService(ELearningDbContext context, UserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }


        public async Task<PaginatedList<AdminUserResponse>> GetAllUsersAsync(int page, int size, string sortBy, string sortDir)
        {
            var query = _context.Users.AsQueryable();

            query = ApplySorting(query, sortBy, sortDir);

            var projected = query.Select(u => new AdminUserResponse
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Gender = u.Gender != null ? u.Gender.ToString() : null,
                Enabled = u.Enabled,
                Role = u.Role.Name,
                CreateAt = u.CreatedAt
            });

            return await PaginatedList<AdminUserResponse>.CreateAsync(projected, page, size);
        }

        public async Task<PaginatedList<AdminUserResponse>> SearchUsersByKeywordsAsync(string[] keywords, int page, int size, string sortBy, string sortDir)
        {
            var query = _context.Users.AsQueryable();

            foreach (var keyword in keywords)
            {
                query = query.Where(u => u.Name.Contains(keyword) || u.Email.Contains(keyword));
            }

            query = ApplySorting(query, sortBy, sortDir);

            var projected = query.Select(u => new AdminUserResponse
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Gender = u.Gender != null ? u.Gender.ToString() : null,
                Enabled = u.Enabled,
                Role = u.Role.Name,
                CreateAt = u.CreatedAt
            });

            return await PaginatedList<AdminUserResponse>.CreateAsync(projected, page, size);
        }

        private IQueryable<User> ApplySorting(IQueryable<User> query, string sortBy, string sortDir)
        {
            if (sortBy.ToLower() == "createdat")
            {
                return sortDir.ToLower() == "asc" ? query.OrderBy(u => u.CreatedAt) : query.OrderByDescending(u => u.CreatedAt);
            }
            else if (sortBy.ToLower() == "name")
            {
                return sortDir.ToLower() == "asc" ? query.OrderBy(u => u.Name) : query.OrderByDescending(u => u.Name);
            }
            else if (sortBy.ToLower() == "email")
            {
                return sortDir.ToLower() == "asc" ? query.OrderBy(u => u.Email) : query.OrderByDescending(u => u.Email);
            }
            return query.OrderByDescending(u => u.CreatedAt);
        }



        public async Task BanUserAsync(long userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            user.Enabled = false;
            await _context.SaveChangesAsync();
        }

        public async Task UnbanUserAsync(long userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            user.Enabled = true;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserRoleAsync(long userId, string roleName)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new Exception("User not found");

            var newRole = await _context.Roles
                .FirstOrDefaultAsync(r => r.Name.ToUpper() == roleName.ToUpper());

            if (newRole == null)
                throw new Exception("Role not found");

            user.RoleId = newRole.Id;
            user.Role = newRole;

            await _context.SaveChangesAsync();
        }
        public async Task<TeacherApplicationDetailResponse> GetUserDetailAsync(long userId)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new Exception("User not found");

            return new TeacherApplicationDetailResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                Gender = user.Gender?.ToString(),
                Avatar = user.Avatar,
                Dob = user.Dob,
                CvUrl = user.CvUrl,
                Certificate = user.Certificate,
                FacebookLink = user.FacebookLink,
                Description = user.Description,
                YearsOfExperience = user.YearsOfExperience,
                Points = user.Points,
                Role = user.Role?.Name ?? "UNKNOWN"
            };
        }



    }

}
