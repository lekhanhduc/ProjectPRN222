namespace E_Learning.Dto.Response.admin
{
    public class AdminReviewDTO
    {
        public long CourseId { get; set; }
        public double AverageRating { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string UserAvatar { get; set; }
        public string Title { get; set; }
        public string Thumbnail { get; set; }
    }
}
