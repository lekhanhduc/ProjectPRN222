namespace E_Learning.Dto.Request
{
    public class PostCreationRequest
    {
        public string Content { get; set; }
        public IFormFile? Image { get; set; }
    }

}
