namespace E_Learning.Dto.Request
{
    public class CommentRequest
    {
        public string Content { get; set; }
        public long? ParentCommentId { get; set; }
        public long PostId { get; set; }
    }
}
