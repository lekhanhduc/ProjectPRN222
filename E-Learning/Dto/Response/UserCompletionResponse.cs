using System;
using System.Collections.Generic;

namespace E_Learning.Dto.Response
{
    public class UserCompletionResponse
    {
        public long TotalLessonComplete { get; set; }
        public long TotalLessons { get; set; }
        public decimal CompletionPercentage { get; set; }

        public List<LessonComplete> LessonCompletes { get; set; } = new List<LessonComplete>();

        public UserCompletionResponse() { }

        public UserCompletionResponse(long totalLessonComplete, long totalLessons, decimal completionPercentage, List<LessonComplete> lessonCompletes)
        {
            TotalLessonComplete = totalLessonComplete;
            TotalLessons = totalLessons;
            CompletionPercentage = completionPercentage;
            LessonCompletes = lessonCompletes ?? new List<LessonComplete>();
        }

        public class LessonComplete
        {
            public long LessonId { get; set; }
            public string LessonName { get; set; }

            public LessonComplete() { }

            public LessonComplete(long lessonId, string lessonName)
            {
                LessonId = lessonId;
                LessonName = lessonName;
            }
        }
    }
}
