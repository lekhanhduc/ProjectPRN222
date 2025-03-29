namespace E_Learning.Dto.Response
{
    public class BuyCourseResponse
    {
        public long CourseId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string CourseLevel { get; set; }

        public string Thumbnail { get; set; }

        public decimal Price { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
