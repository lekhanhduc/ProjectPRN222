using E_Learning.Common;
using E_Learning.Data;
using E_Learning.Dto.Request;
using E_Learning.Dto.Response;
using E_Learning.Middlewares;
using E_Learning.Repositories;
using E_Learning.Servies;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Services.Impl
{
    public class RevenueService : IRevenueService
    {
        private readonly PaymentRepository paymentRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserRepository userRepository;
        private readonly ELearningDbContext _context;

        public RevenueService(PaymentRepository paymentRepository, IHttpContextAccessor httpContextAccessor, UserRepository userRepository, ELearningDbContext _context)
        {
            this.paymentRepository = paymentRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.userRepository = userRepository;
            this._context = _context;
        }

        public async Task<RevenueSummaryResponse> RevenueTeacher(PeriodTypeRequest request)
        {
            // Lấy userId từ claims của người dùng đang đăng nhập
            var userIdClaim = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userIdClaim == null)
            {
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }

            var user = await userRepository.FindUserById(long.Parse(userIdClaim.Value));
            if (user == null)
            {
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }

            List<RevenueDetailResponse> revenueDetails = new List<RevenueDetailResponse>();
            decimal totalRevenue = 0m;

            // Xử lý khi PeriodType là YEAR và không có Month
            if (request.PeriodType == PeriodType.YEAR && request.Year.HasValue && !request.Month.HasValue)
            {
                int year = request.Year.Value;
                for (int month = 1; month <= 12; month++)
                {
                    DateTime start = new DateTime(year, month, 1);
                    DateTime end = start.AddMonths(1).AddDays(-1);

                    var payments = await paymentRepository
                        .FindPaymentsByAuthorStatusAndDateRangeAsync(user, PaymentStatus.SUCCESS, start, end);

                    decimal monthRevenue = payments.Sum(p => p.Price);
                    totalRevenue += monthRevenue;

                    revenueDetails.Add(new RevenueDetailResponse
                    {
                        Month = month,
                        Year = year,
                        Revenue = monthRevenue
                    });
                }
            }
            // Xử lý khi PeriodType là YEAR và có Month
            else if (request.PeriodType == PeriodType.YEAR && request.Year.HasValue && request.Month.HasValue)
            {
                int year = request.Year.Value;
                int month = request.Month.Value;

                DateTime start = new DateTime(year, month, 1);
                DateTime endOfMonth = start.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);

                int totalWeeks = (int)Math.Ceiling(endOfMonth.Day / 7.0);
                for (int week = 1; week <= totalWeeks; week++)
                {
                    DateTime end = start.AddDays(7).AddSeconds(-1);
                    if (end > endOfMonth)
                    {
                        end = endOfMonth;
                    }

                    var payments = await paymentRepository
                        .FindPaymentsByAuthorStatusAndDateRangeAsync(user, PaymentStatus.SUCCESS, start, end);

                    Console.WriteLine(payments);
                    decimal weekRevenue = payments.Sum(p => p.Price);
                    totalRevenue += weekRevenue;
                    Console.WriteLine(totalRevenue);
                    revenueDetails.Add(new RevenueDetailResponse
                    {
                        Month = month,
                        Year = year,
                        Week = week,
                        Revenue = weekRevenue
                    });

                    start = end.AddSeconds(1);
                }
            }

            return new RevenueSummaryResponse
            {
                TotalRevenue = totalRevenue,
                RevenueDetails = revenueDetails
            };
        }

    }
}
