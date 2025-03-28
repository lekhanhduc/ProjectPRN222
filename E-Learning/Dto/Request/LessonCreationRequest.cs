namespace E_Learning.Dto.Request
{
    public class LessonCreationRequest
    {
        public long CourseId { get; set; }
        public long ChapterId { get; set; }
        public string LessonName { get; set; }
        public string? Description { get; set; }

        public IFormFile Video { get; set; }
    }
}
