using E_Learning.Dto.Request;
using E_Learning.Dto.Response;
using E_Learning.Dto.Response.Admin;
using E_Learning.Servies.admin;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.Controllers.admin
{
    [Route("api/v1/admin/salary")]
    [ApiController]
    public class AdminSalaryController : ControllerBase
    {
        private readonly AdminSalaryService _adminSalaryService;

        // Constructor để inject service AdminSalaryService
        public AdminSalaryController(AdminSalaryService adminSalaryService)
        {
            _adminSalaryService = adminSalaryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSalaries(
            [FromQuery] int? year,
            [FromQuery] int? month,
            [FromQuery] int page = 1,
            [FromQuery] int size = 10,
            [FromQuery] string sort = "createdAt,asc"
            )
        {
            var sortArgs = sort.Split(',');
            string sortBy = sortArgs[0];
            string sortDir = sortArgs.Length > 1 ? sortArgs[1] : "asc";
            try
            {
                // Gọi service để lấy dữ liệu salary
                var result = await _adminSalaryService.GetSalaries(year, month, page, size, sortBy);

                // Trả về kết quả phân trang
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Trả về lỗi nếu có vấn đề trong quá trình xử lý
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveSalary([FromBody] AdminSalaryRequest salaryRequest)
        {
            try
            {
                // Gọi service để lưu salary vào cơ sở dữ liệu
                await _adminSalaryService.SaveSalary(salaryRequest);

                // Trả về kết quả thành công
                return Ok(new { message = "Salary saved successfully." });
            }
            catch (Exception ex)
            {
                // Trả về lỗi nếu có vấn đề trong quá trình lưu
                return BadRequest(new { message = ex.Message });
            }


        }

        [HttpGet("completed")]
        public async Task<IActionResult> GetCompletedSalaries(
            [FromQuery] int year = 2025,
            [FromQuery] int month = 3,
            [FromQuery] int page = 1,
            [FromQuery] int size = 10,
            [FromQuery] string sort = "createdAt,asc")
        {
            var sortArgs = sort.Split(',');
            string sortBy = sortArgs[0];
            string sortDir = sortArgs.Length > 1 ? sortArgs[1] : "asc";
            try
            {
                // Gọi service để lấy danh sách lương đã hoàn thành
                var result = await _adminSalaryService.GetCompletedSalaries(year, month, page, size, sortBy, sortDir);

                // Trả về kết quả thành công
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Trả về lỗi nếu có vấn đề trong quá trình lấy dữ liệu
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchSalaries(
            [FromQuery] string[] keywords,
            [FromQuery] int? year,
            [FromQuery] int? month,
            [FromQuery] int page = 1,
            [FromQuery] int size = 10,
            [FromQuery] string sort = "createdAt,asc")
        {
            var sortArgs = sort.Split(',');
            string sortBy = sortArgs[0];
            string sortDir = sortArgs.Length > 1 ? sortArgs[1] : "asc";
            try
            {
                // Gọi service để tìm kiếm Salary
                var result = await _adminSalaryService.SearchSalaries(keywords, year, month, page, size, sortBy, sortDir);

                // Trả về kết quả tìm kiếm
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Trả về lỗi nếu có vấn đề trong quá trình tìm kiếm
                return BadRequest(new { message = ex.Message });
            }
        }
    }
} 
