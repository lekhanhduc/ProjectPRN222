namespace E_Learning.Dto.Response
{
    public class CourseCreationResponse
    {
        public long Id { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public string Language { get; set; }
        public string CourseLevel { get; set; }
        public string Thumbnail { get; set; }
        public decimal Price { get; set; }
    }
}
