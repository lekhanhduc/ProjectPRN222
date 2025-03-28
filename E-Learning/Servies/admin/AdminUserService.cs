using E_Learning.Data;
using E_Learning.Dto.Response;
using E_Learning.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Services.admin
{
    public class AdminUserService : IAdminUserService
    {
        private readonly ELearningDbContext _context;

        public AdminUserService(ELearningDbContext context)
        {
            _context = context;
        }

        public PagedResult<UserResponse> GetAllUsers(int page, int size, string sortBy, bool isDescending)
        {
            var query = _context.Users.Include(u => u.Role).AsQueryable();

            // Sorting
            query = isDescending
                ? query.OrderByDescending(u => EF.Property<object>(u, sortBy))
                : query.OrderBy(u => EF.Property<object>(u, sortBy));

            var pagedUsers = query.Skip((page - 1) * size).Take(size).Select(u => new UserResponse
            {
                Email = u.Email,
                Name = u.Name,
                FirstName = u.FirstName,
                LastName = u.LastName
            }).ToList();

            var total = query.Count();

            return new PagedResult<UserResponse>(pagedUsers, total, page, size);
        }

        public PagedResult<UserResponse> SearchUsers(string keywords, int page, int size, string sortBy, bool isDescending)
        {
            var keywordArray = keywords.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var query = _context.Users.Include(u => u.Role)
                .Where(u => keywordArray.Any(kw =>
                    u.Name.Contains(kw) ||
                    u.Email.Contains(kw) ||
                    u.FirstName.Contains(kw) ||
                    u.LastName.Contains(kw)));

            // Xử lý sorting hợp lệ và an toàn
            query = sortBy?.ToLower() switch
            {
                "email" => isDescending ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email),
                "firstname" => isDescending ? query.OrderByDescending(u => u.FirstName) : query.OrderBy(u => u.FirstName),
                "lastname" => isDescending ? query.OrderByDescending(u => u.LastName) : query.OrderBy(u => u.LastName),
                _ => isDescending ? query.OrderByDescending(u => u.Name) : query.OrderBy(u => u.Name),
            };

            // Lấy total trước phân trang
            var total = query.Count();

            // phân trang
            var pagedUsers = query.Skip((page - 1) * size).Take(size).Select(u => new UserResponse
            {
                Email = u.Email,
                Name = u.Name,
                FirstName = u.FirstName,
                LastName = u.LastName
            }).ToList();

            return new PagedResult<UserResponse>(pagedUsers, total, page, size);
        }


        public void BanUser(long userId)
        {
            var user = _context.Users.Find(userId);
            if (user == null) throw new Exception("User not found");
            user.Enabled = false;
            _context.SaveChanges();
        }

        public void UnbanUser(long userId)
        {
            var user = _context.Users.Find(userId);
            if (user == null) throw new Exception("User not found");
            user.Enabled = true;
            _context.SaveChanges();
        }

        public void UpdateUserRole(long userId, string roleName)
        {
            var user = _context.Users.Find(userId);
            var role = _context.Roles.FirstOrDefault(r => r.Name == roleName);
            if (user == null || role == null) throw new Exception("User or role not found");
            user.RoleId = role.Id;
            _context.SaveChanges();
        }

        //public TeacherApplicationDetailResponse GetUserApplicationDetail(long userId)
        //{
        //    var user = _context.Users.Include(u => u.Role).FirstOrDefault(u => u.Id == userId);
        //    if (user == null) throw new Exception("User not found");

        //    return new TeacherApplicationDetailResponse
        //    {
        //        Id = user.Id,
        //        Name = user.Name,
        //        Email = user.Email,
        //        Phone = user.Phone,
        //        Gender = user.Gender?.ToString(),
        //        Avatar = user.Avatar,
        //        Dob = user.Dob,
        //        Description = user.Description,
        //        Role = user.Role.Name
        //        // Thêm các trường khác tùy theo nhu cầu của bạn
        //    };
        //}
    }
}
