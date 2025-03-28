
namespace E_Learning.Dto.Response
{
    public class ChapterCreationResponse
    {
        public string UserName { get; set; }
        public long CourseId { get; set; }
        public long ChapterId { get; set; }
        public string? ChapterName { get; set; }
        public string? Description { get; set; }

        public HashSet<LessonDtoCreateChapter>? Lessons { get; set; } = [];
    }
}
