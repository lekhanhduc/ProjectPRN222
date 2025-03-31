namespace E_Learning.Dto.Response
{
    public class RevenueSummaryResponse
    {
        public decimal TotalRevenue { get; set; }
        public List<RevenueDetailResponse>? RevenueDetails { get; set; }
    }
}
