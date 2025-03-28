namespace E_Learning.Dto.Response
{
    public class LessonProgressResponse
    {
        public long LessonId { get; set; }
        public string LessonName { get; set; }
        public bool IsComplete { get; set; }
    }
}
