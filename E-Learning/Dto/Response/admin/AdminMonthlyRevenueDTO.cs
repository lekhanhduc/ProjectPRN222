namespace E_Learning.Dto.Response.admin
{
    public class AdminMonthlyRevenueDTO
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal TotalRevenue { get; set; }
        public long TotalCoursesSold { get; set; }
    }
}
