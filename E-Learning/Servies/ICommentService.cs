using E_Learning.Dto.Request;
using E_Learning.Dto.Response;

namespace E_Learning.Servies
{
    public interface ICommentService
    {
        Task<PageResponse<CommentResponse>> GetCommentByPostId(long postId, int page, int size);
        Task<CommentResponse> AddComment(CommentRequest request);
        Task DeleteComment(long commentId);
    }
}
