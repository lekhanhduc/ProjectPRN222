using E_Learning.Data;
using E_Learning.Dto.Response.admin;
using E_Learning.Entity;
using E_Learning.Repositories;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Servies.admin
{
    public class AdminRevenueService : IAdminRevenueService
    {
        private readonly PaymentRepository _paymentRepository;
        private readonly EnrollmentRepository _enrollmentRepository;

        public AdminRevenueService(PaymentRepository paymentRepository, EnrollmentRepository enrollmentRepository)
        {
            _paymentRepository = paymentRepository;
            _enrollmentRepository = enrollmentRepository;
        }

        public List<AdminTeacherRevenueDTO> GetTeacherRevenueByYear(int year, bool ascending)
        {
            var payments = _paymentRepository.GetPaymentsByYear(year);
            return GenerateTeacherRevenue(payments, ascending);
        }

        public List<AdminTeacherRevenueDTO> GetTeacherRevenueByMonth(int month, int year, bool ascending)
        {
            var payments = _paymentRepository.GetPaymentsByMonth(month, year);
            return GenerateTeacherRevenue(payments, ascending);
        }

        private List<AdminTeacherRevenueDTO> GenerateTeacherRevenue(List<Payment> payments, bool ascending)
        {
            var map = new Dictionary<long, AdminTeacherRevenueDTO>();

            foreach (var payment in payments)
            {
                var course = payment.Course;
                var teacher = course?.Author;

                if (teacher != null && teacher.RoleId == 3)
                {
                    if (!map.ContainsKey(teacher.Id))
                    {
                        map[teacher.Id] = new AdminTeacherRevenueDTO
                        {
                            TeacherId = teacher.Id,
                            TeacherName = teacher.FullName,
                            TotalRevenue = 0,
                            TotalCoursesSold = 0,
                            TeacherAvatar = teacher.Avatar
                        };
                    }

                    map[teacher.Id].TotalRevenue += payment.Price;
                    map[teacher.Id].TotalCoursesSold += 1;
                }
            }

            var list = map.Values.ToList();
            list = ascending
                ? list.OrderBy(x => x.TotalRevenue).ThenBy(x => x.TotalCoursesSold).ToList()
                : list.OrderByDescending(x => x.TotalRevenue).ThenByDescending(x => x.TotalCoursesSold).ToList();

            return list;
        }

        public List<AdminUserBuyDTO> GetUserBuyStatsByMonth(int month, int year, bool ascending)
        {
            var result = _enrollmentRepository.GetUserBuyStatsByMonth(month, year);
            return ascending
                ? result.OrderBy(x => x.TotalCoursesBought).ToList()
                : result.OrderByDescending(x => x.TotalCoursesBought).ToList();
        }

        public List<AdminUserBuyDTO> GetUserBuyStatsByYear(int year, bool ascending)
        {
            var result = _enrollmentRepository.GetUserBuyStatsByYear(year);
            return ascending
                ? result.OrderBy(x => x.TotalCoursesBought).ToList()
                : result.OrderByDescending(x => x.TotalCoursesBought).ToList();
        }

        public List<AdminMonthlyRevenueDTO> GetMonthlyRevenue(int year)
        {
            return _paymentRepository.GetMonthlyRevenue(year);
        }

    }
}
