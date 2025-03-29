using E_Learning.Common;
using E_Learning.Dto.Request;
using E_Learning.Dto.Response;
using E_Learning.Entity;
using E_Learning.Middlewares;
using E_Learning.Repositories;

namespace E_Learning.Servies.Impl
{
    public class CommentService : ICommentService
    {

        private readonly CommentRepository commentRepository;
        private readonly PostRepository postRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserRepository userRepository;


        public CommentService(CommentRepository commentRepository, PostRepository postRepository, IHttpContextAccessor httpContextAccessor, UserRepository userRepository)
        {
            this.commentRepository = commentRepository;
            this.postRepository = postRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.userRepository = userRepository;
        }

        public async Task<PageResponse<CommentResponse>> GetCommentByPostId(long postId, int page, int size)
        {
            page = page < 1 ? 1 : page;
            size = size < 1 ? 4 : size;

            var parentComments = await commentRepository.FindCommentByPostIdAndParentCommentIsNull(postId, page, size);

            var totalCount = await commentRepository.CountCommentsByPostIdAndParentCommentIsNull(postId);

            var responses = parentComments.Select(comment =>
            {
                var response = new CommentResponse
                {
                    Id = comment.Id,
                    PostId = comment.PostId.ToString(),
                    Name = comment.User?.Name,
                    Avatar = comment.User?.Avatar,
                    Content = comment.Content,
                    CreatedAt = comment.CreatedAt,
                    Replies = comment.Replies?.Select(reply => new CommentResponse
                    {
                        Id = reply.Id,
                        PostId = reply.PostId.ToString(),
                        Name = reply.User?.Name,
                        Avatar = reply.User?.Avatar,
                        Content = reply.Content,
                        CreatedAt = reply.CreatedAt,
                        //Replies = new List<CommentResponse>()
                    }).ToList() ?? new List<CommentResponse>()
                };
                return response;
            }).ToList();

            int totalPages = (int)Math.Ceiling((double)totalCount / size);

            return new PageResponse<CommentResponse>
            {
                CurrentPage = page,
                PageSize = size,
                TotalElemets = totalCount,
                TotalPages = totalPages,
                Data = responses
            };
        }

        public async Task<CommentResponse> AddComment(CommentRequest request)
        {
            var userId = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userId == null)
            {
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }

            var user = await userRepository.FindUserById(long.Parse(userId.Value));

            if (user == null)
            {
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }
            var post = await postRepository.GetById(request.PostId);
            if (post == null)
            {
                throw new AppException(ErrorCode.POST_NOT_EXISTED);
            }

            Comment parentComment = null;
            if (request.ParentCommentId.HasValue)
            {
                parentComment = await commentRepository.FindById(request.ParentCommentId.Value);
                if (parentComment == null)
                {
                    throw new AppException(ErrorCode.PARENT_COMMENT_NOT_EXISTED);
                }
            }

            if (string.IsNullOrEmpty(request.Content))
            {
                throw new AppException(ErrorCode.CONTENT_COMMENT_INVALID);
            }

            var comment = new Comment
            {
                Content = request.Content,
                User = user,
                UserId = user.Id,
                Post = post,
                PostId = post.Id,
                ParentComment = parentComment,
                ParentCommentId = parentComment?.Id,
                CreatedAt = DateTime.UtcNow
            };

            await commentRepository.Save(comment);
            return new CommentResponse
            {
                Id = comment.Id,
                PostId = comment.PostId.ToString(),
                Name = user.Name, 
                Avatar = user.Avatar, 
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                Replies = new List<CommentResponse>()
            };
        }

        public async Task DeleteComment(long commentId)
        {
            var userIdClaim = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userIdClaim == null)
            {
                throw new AppException(ErrorCode.UNAUTHORIZED);
            }

            var user = await userRepository.FindUserById(long.Parse(userIdClaim.Value));
            if (user == null)
            {
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }

            var comment = await commentRepository.FindById(commentId);
            if (comment == null)
            {
                throw new AppException(ErrorCode.COMMENT_NOT_EXISTED);
            }

            if (user.Id == comment.UserId || user.Role.Name == DefinitionRole.ADMIN)
            {
                await commentRepository.Delete(comment);
                return;
            }

            throw new AppException(ErrorCode.DELETE_COMMENT_INVALID);
        }

    }
}
