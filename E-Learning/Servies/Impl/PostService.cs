using E_Learning.Dto.Request;
using E_Learning.Dto.Response;
using E_Learning.Entity;
using E_Learning.Middlewares;
using E_Learning.Repositories;
using Elastic.Clients.Elasticsearch.Security;

namespace E_Learning.Servies.Impl
{
    public class PostService : IPostService
    {
        private readonly PostRepository postRepository;
        private readonly CloudinaryService cloudinaryService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserRepository userRepository;


        public PostService(PostRepository postRepository, CloudinaryService cloudinaryService, IHttpContextAccessor httpContextAccessor, UserRepository userRepository)
        {
            this.postRepository = postRepository;
            this.cloudinaryService = cloudinaryService;
            this.httpContextAccessor = httpContextAccessor;
            this.userRepository = userRepository;
        }

        public async Task<PostCreationResponse> CreatePost(PostCreationRequest request)
        {
            var userId = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userId == null)
            {
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }

            var user = await userRepository.FindUserById(int.Parse(userId.Value));
            if (user == null)
            {
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }
            string imageUrl = null;
            if (request.Image != null)
            {
                using var stream = request.Image.OpenReadStream();
                imageUrl = await cloudinaryService.UploadImageAsync(stream, request.Image.Name);
            }

            var post = new Post
            {
                Title = request.Content,
                Content = request.Content,
                Image = imageUrl,
                UserId = int.Parse(userId.Value)
            };

            await postRepository.Create(post);

            return new PostCreationResponse
            {
                Id = post.Id,
                Name = user.Name,
                Avatar = user.Avatar,
                Content = post.Content,
                Image = post.Image,
                CreatedAt = post.CreatedAt
            };
        }

        public async Task<PageResponse<PostResponse>> GetPost(int page, int size)
        {
            var userId = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userId == null)
            {
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }

            var user = await userRepository.FindUserById(int.Parse(userId.Value));
            if (user == null)
            {
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }

            var posts = await postRepository.FindAll(page, size);
            var totalPosts = await postRepository.CountAllPost();

            var postResponses = posts.Select(post => new PostResponse
            {
                Id = post.Id,
                Name = post.User.Name,
                Avatar = post.User?.Avatar,
                Content = post.Content,
                Image = post.Image,
                Owner = post.UserId == user.Id // Kiểm tra quyền sở hữu bài đăng
            }).ToList();

            return new PageResponse<PostResponse>
            {
                CurrentPage = page,
                PageSize = size,
                TotalElemets = totalPosts,
                TotalPages = (int)Math.Ceiling((double)totalPosts / size),
                Data = postResponses
            };
        }

    }
}
