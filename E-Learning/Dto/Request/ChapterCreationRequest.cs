namespace E_Learning.Dto.Request
{
    public class ChapterCreationRequest
    {
        public long CourseId { get; set; }
        public string ChapterName { get; set; }
        public string Description { get; set; }
    }
}
