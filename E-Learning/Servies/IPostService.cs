using E_Learning.Dto.Request;
using E_Learning.Dto.Response;

namespace E_Learning.Servies
{
    public interface IPostService
    {
        Task<PostCreationResponse> CreatePost(PostCreationRequest request);
        Task<PageResponse<PostResponse>> GetPost(int page, int size);
    }
}
