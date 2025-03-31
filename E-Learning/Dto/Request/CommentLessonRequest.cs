namespace E_Learning.Dto.Request
{
    public class CommentLessonRequest
    {
        public long CourseId { get; set; }
        public long ChapterId { get; set; }
        public long LessonId { get; set; }
        public string Content { get; set; }
        public long? ParentReviewId { get; set; }
    }
}
