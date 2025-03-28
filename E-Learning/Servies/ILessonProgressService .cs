using E_Learning.Dto.Request;
using E_Learning.Dto.Response;

namespace E_Learning.Servies
{
    public interface ILessonProgressService
    {
        Task<UserCompletionResponse> CalculateCompletion(long courseId);
        Task<LessonProgressResponse> MarkLessonAsCompleted(LessonProgressRequest request);

    }
}
