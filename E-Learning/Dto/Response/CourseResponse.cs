using E_Learning.Common;

namespace E_Learning.Models.Response
{
    public class CourseResponse
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public Level Level { get; set; }
        public string Language { get; set; }
        public decimal Point { get; set; }
    }
}
