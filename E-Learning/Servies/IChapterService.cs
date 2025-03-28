using E_Learning.Dto.Request;
using E_Learning.Dto.Response;

namespace E_Learning.Servies
{
    public interface IChapterService
    {
        Task<ChapterCreationResponse> CreateChapter(ChapterCreationRequest request);
    }
}
