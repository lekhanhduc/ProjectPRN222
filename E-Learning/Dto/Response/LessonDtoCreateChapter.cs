namespace E_Learning.Dto.Response
{
    public class LessonDtoCreateChapter
    {
        public long LessonId { get; set; }
        public string? LessonName { get; set; }
        public string? VideoUrl { get; set; }
        public string? LessonDescription { get; set; }
    }
}
