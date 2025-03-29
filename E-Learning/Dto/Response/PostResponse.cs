namespace E_Learning.Dto.Response
{
    public class PostResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public string? Avatar { get; set; }
        public string Content { get; set; }
        public string? Image { get; set; }
        public bool Owner { get; set; }
    }
}
