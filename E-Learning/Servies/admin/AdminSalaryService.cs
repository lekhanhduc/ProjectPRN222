using E_Learning.Common;
using E_Learning.Data;
using E_Learning.Dto.Request;
using E_Learning.Dto.Response;
using E_Learning.Dto.Response.Admin;
using E_Learning.Entity;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Servies.admin
{
    public class AdminSalaryService
    {
        private readonly ELearningDbContext _context;

        public AdminSalaryService(ELearningDbContext context)
        {
            _context = context;
        }

        // Phương thức lấy danh sách payment theo tháng, năm, tính tổng tiền theo từng author với phân trang
        public async Task<PagedResult<AdminSalaryResponse>> GetSalaries(int? year, int? month, int page, int size, string sortBy = "createdAt", string sortDir = "asc")
        {
            // Lấy danh sách Payment theo tháng, năm và bao gồm thông tin về course và author
            var query = _context.Payments
                .Where(p => p.CreatedAt.Month == month && p.CreatedAt.Year == year && p.Status == PaymentStatus.SUCCESS)
                .Include(p => p.Course) // Lấy thông tin về khóa học (course)
                .ThenInclude(c => c.Author) // Lấy thông tin tác giả (author) của khóa học
                .AsQueryable();

            // Kiểm tra xem AuthorId, Year, và Month đã tồn tại trong bảng Salary chưa
            var existingSalaryQuery = _context.Salaries
                .Where(s => s.AuthorId.HasValue && s.CreatedAt.Year == year && s.CreatedAt.Month == month);

            var existingSalaries = await existingSalaryQuery.ToListAsync();
            var existingAuthorIds = existingSalaries.Select(s => s.AuthorId).ToList();

            // Sắp xếp theo các trường khác nhau
            if (sortBy.ToLower() == "createdat")
            {
                query = sortDir.ToLower() == "desc"
                    ? query.OrderByDescending(p => p.CreatedAt)
                    : query.OrderBy(p => p.CreatedAt);
            }
            else if (sortBy.ToLower() == "authorname")
            {
                query = sortDir.ToLower() == "desc"
                    ? query.OrderByDescending(p => p.Course.Author.Name)
                    : query.OrderBy(p => p.Course.Author.Name);
            }
            else
            {
                query = query.OrderBy(p => p.CreatedAt); // Sắp xếp theo CreatedAt mặc định
            }

            // Tính tổng tiền cho từng author từ các payment (dựa vào course và author)
            var authorTotalMoney = query
                .GroupBy(p => p.Course.Author.Id) // Nhóm theo authorId
                .Select(g => new AdminSalaryResponse
                {
                    AuthorId = g.Key,
                    Money = g.Sum(p => p.Price), // Tổng tiền của author từ các payment
                    TeacherName = g.First().Course.Author.Name ?? string.Empty, // Lấy author đầu tiên trong nhóm
                    TeacherEmail = g.First().Course.Author.Email ?? string.Empty,
                    Qr = g.First().Course.Author.Qr ?? string.Empty,
                    CreatedAt = g.First().CreatedAt,
                    Status = TransactionStatus.PROCESSING.ToString()
                })
                .Where(response => !existingAuthorIds.Contains(response.AuthorId)); // Kiểm tra nếu authorId đã có trong danh sách tồn tại

            // Phân trang kết quả
            var pagedResult = await PagedResult<AdminSalaryResponse>.CreateAsync(
                authorTotalMoney.AsQueryable(), page, size, sortBy, sortDir);

            return pagedResult;
        }


        public async Task SaveSalary(AdminSalaryRequest salaryRequest)
        {
            // Kiểm tra AuthorId có tồn tại trong bảng Users
            var author = await _context.Users.FindAsync(salaryRequest.AuthorId);

            if (author == null)
            {
                throw new Exception("Author not found");
            }

            // Tạo mới đối tượng Salary
            var salary = new Salary
            {
                AuthorId = salaryRequest.AuthorId,
                Money = salaryRequest.Money,
                Status = TransactionStatus.COMPLETED, // Trạng thái mặc định
                CreatedAt = salaryRequest.CreatedAt ?? DateTime.UtcNow // Ngày tạo mặc định là ngày hiện tại
            };

            // Lưu vào bảng Salaries
            await _context.Salaries.AddAsync(salary);
            await _context.SaveChangesAsync(); // Lưu vào cơ sở dữ liệu
        }

        public async Task<PagedResult<AdminSalaryResponse>> GetCompletedSalaries(
            int year,
            int month,
            int page = 1,
            int size = 10,
            string sortBy = "createdAt",
            string sortDir = "asc")
        {
            // Lấy danh sách các Salary đã hoàn thành với trạng thái "Completed"
            var query = _context.Salaries
                .Where(s => s.Status == TransactionStatus.COMPLETED)
                .Include(s => s.Author) // Lấy thông tin tác giả (author)
                .Where(s => s.CreatedAt.Month == month && s.CreatedAt.Year == year);


            // Sắp xếp theo các trường khác nhau
            if (sortBy.ToLower() == "createdat")
            {
                query = sortDir.ToLower() == "desc"
                    ? query.OrderByDescending(s => s.CreatedAt)
                    : query.OrderBy(s => s.CreatedAt);
            }
            else if (sortBy.ToLower() == "authorname")
            {
                query = sortDir.ToLower() == "desc"
                    ? query.OrderByDescending(s => s.Author.Name)
                    : query.OrderBy(s => s.Author.Name);
            }
            else
            {
                query = query.OrderBy(s => s.CreatedAt); // Sắp xếp theo CreatedAt mặc định
            }

            // Đếm tổng số bản ghi
            var count = await query.CountAsync();

            // Lấy các bản ghi trong phạm vi phân trang
            var salaryList = await query.Skip((page - 1) * size).Take(size).ToListAsync();

            // Chuyển dữ liệu sang AdminSalaryResponse để trả về cho client
            var salaryResponses = salaryList.Select(s => new AdminSalaryResponse
            {
                AuthorId = s.AuthorId,
                TeacherName = s.Author.Name,
                Money = s.Money,
                Status = s.Status.ToString(),
                CreatedAt = s.CreatedAt,
                TeacherEmail = s.Author.Email,
                Qr = s.Author.Qr
            }).ToList();

            // Trả về kết quả phân trang
            return new PagedResult<AdminSalaryResponse>(salaryResponses, count, page, size);
        }

        public async Task<PagedResult<AdminSalaryResponse>> SearchSalaries(
            string[] keywords,
            int? year,
            int? month,
            int page = 1,
            int size = 10,
            string sortBy = "createdAt",
            string sortDir = "asc")
        {
            // Bắt đầu với câu truy vấn Salary
            var query = _context.Salaries
                .Include(s => s.Author) // Lấy thông tin tác giả (author)
                .AsQueryable();

            // Lọc theo tháng và năm nếu có
            if (year.HasValue && month.HasValue)
            {
                query = query.Where(s => s.CreatedAt.Year == year && s.CreatedAt.Month == month);
            }
            else if (year.HasValue)
            {
                query = query.Where(s => s.CreatedAt.Year == year);
            }
            else if (month.HasValue)
            {
                query = query.Where(s => s.CreatedAt.Month == month);
            }

            // Kiểm tra nếu có từ khóa tìm kiếm, tìm kiếm theo tên và email tác giả
            if (keywords != null && keywords.Length > 0)
            {
                foreach (var keyword in keywords)
                {
                    query = query.Where(s => s.Author.Name.Contains(keyword) || s.Author.Email.Contains(keyword));
                }
            }

            // Sắp xếp theo các trường khác nhau
            if (sortBy.ToLower() == "createdat")
            {
                query = sortDir.ToLower() == "desc"
                    ? query.OrderByDescending(s => s.CreatedAt)
                    : query.OrderBy(s => s.CreatedAt);
            }
            else if (sortBy.ToLower() == "authorname")
            {
                query = sortDir.ToLower() == "desc"
                    ? query.OrderByDescending(s => s.Author.Name)
                    : query.OrderBy(s => s.Author.Name);
            }
            else
            {
                query = query.OrderBy(s => s.CreatedAt); // Sắp xếp theo CreatedAt mặc định
            }

            // Đếm tổng số bản ghi
            var count = await query.CountAsync();

            // Lấy các bản ghi trong phạm vi phân trang
            var salaryList = await query.Skip((page - 1) * size).Take(size).ToListAsync();

            // Chuyển dữ liệu sang AdminSalaryResponse để trả về cho client
            var salaryResponses = salaryList.Select(s => new AdminSalaryResponse
            {
                AuthorId = s.AuthorId,
                TeacherName = s.Author.Name,
                Money = s.Money,
                Status = s.Status.ToString(),
                CreatedAt = s.CreatedAt,
                TeacherEmail = s.Author.Email,
                Qr = s.Author.Qr
            }).ToList();

            // Trả về kết quả phân trang
            return new PagedResult<AdminSalaryResponse>(salaryResponses, count, page, size);
        }
    }
}
