using E_Learning.Dto.Request;
using E_Learning.Dto.Response;

namespace E_Learning.Servies
{
    public interface ILessonService
    {
        Task<LessonCreationResponse> CreateLesson(LessonCreationRequest request);
        Task DeleteLesson(long lessonId);
    }
}
