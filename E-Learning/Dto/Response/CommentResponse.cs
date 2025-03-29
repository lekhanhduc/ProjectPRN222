namespace E_Learning.Dto.Response
{
    public class CommentResponse
    {
        public long Id { get; set; }
        public string PostId { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<CommentResponse> Replies { get; set; } = new List<CommentResponse>();
    }
}
