namespace E_Learning.Dto.Request
{
    public class CourseCreationRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public string Language { get; set; }
        public string CourseLevel { get; set; }
        public decimal Price { get; set; }
        public IFormFile Thumbnail { get; set; }
    }
}
