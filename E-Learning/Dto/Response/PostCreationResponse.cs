namespace E_Learning.Dto.Response
{
    public class PostCreationResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? Avatar { get; set; }
        public string Content { get; set; }
        public string? Image { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
