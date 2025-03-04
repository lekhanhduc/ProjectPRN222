namespace E_Learning.Models.Response
{
    public class CourseResponse
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public string Level { get; set; }
        public string Language { get; set; }
        public decimal Price { get; set; }
        public string Thumbnail { get; set; }
    }
}
