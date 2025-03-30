using E_Learning.Dto.Response.admin;

namespace E_Learning.Servies.admin
{
    public interface IAdminRevenueService
    {
        //List<AdminMonthlyRevenueDTO> GetMonthlyRevenue(int year);
        List<AdminTeacherRevenueDTO> GetTeacherRevenueByYear(int year, bool ascending);
        List<AdminTeacherRevenueDTO> GetTeacherRevenueByMonth(int month, int year, bool ascending);
        //List<AdminUserBuyDTO> GetUserBuyStatsByMonth(int month, int year, bool ascending);
        //List<AdminUserBuyDTO> GetUserBuyStatsByYear(int year, bool ascending);
    }
}
