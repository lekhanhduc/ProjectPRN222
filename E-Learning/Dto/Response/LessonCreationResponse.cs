namespace E_Learning.Dto.Response
{
    public class LessonCreationResponse
    {
        public long CourseId { get; set; }
        public long ChapterId { get; set; }
        public long LessonId { get; set; }

        public string LessonName { get; set; }

        public string? VideoUrl { get; set; }
        public string? LessonDescription { get; set; }
    }
}
