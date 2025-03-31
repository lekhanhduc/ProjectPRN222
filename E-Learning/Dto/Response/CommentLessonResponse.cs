namespace E_Learning.Dto.Response
{
    public class CommentLessonResponse
    {
        public long ReviewId { get; set; }
        public long CourseId { get; set; }
        public long ChapterId { get; set; }
        public long LessonId { get; set; }
        public string Name { get; set; }
        public string? Avatar { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<CommentLessonResponse> Replies { get; set; }
    }
}
