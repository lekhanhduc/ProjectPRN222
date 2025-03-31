using E_Learning.Common;

public class AdsApproveResponse
{
    public long Id { get; set; }
    public string ContactEmail { get; set; }
    public string ContactPhone { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }  // Thay vì ImageUrl
    public string Link { get; set; }
    public DateOnly StartDate { get; set; }  // Sử dụng DateOnly thay vì DateTime
    public DateOnly EndDate { get; set; }  // Sử dụng DateOnly thay vì DateTime
    public decimal PriceAds { get; set; }
    public AdsStatus Status { get; set; }
}
