namespace E_Learning.Dto.Response
{
    using System.Collections.Generic;

    namespace E_Learning.Dto.Response
    {
        public class CourseChapterResponse
        {
            public long CourseId { get; set; }
            public long TotalLesson { get; set; }
            public string CourseTitle { get; set; }
            public string CourseDescription { get; set; }

            public HashSet<ChapterDto> Chapters { get; set; } = new();

            public class ChapterDto
            {
                public long ChapterId { get; set; }
                public string ChapterName { get; set; }

                public HashSet<LessonDto> LessonDto { get; set; } = new();
            }

            public class LessonDto
            {
                public long LessonId { get; set; }
                public string? LessonName { get; set; }
                public string? Description { get; set; }
                public string? VideoUrl { get; set; }
            }
        }
    }

}
