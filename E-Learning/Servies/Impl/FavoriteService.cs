using E_Learning.Dto.Request;
using E_Learning.Dto.Response;
using E_Learning.Entity;
using E_Learning.Middlewares;
using E_Learning.Repositories;

namespace E_Learning.Servies.Impl
{
    public class FavoriteService : IFavoriteService
    {

        private readonly FavoriteRepository favoriteRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly CourseRepository courseRepository;

        public FavoriteService(FavoriteRepository favoriteRepository, IHttpContextAccessor httpContextAccessor, CourseRepository courseRepository)
        {
            this.favoriteRepository = favoriteRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.courseRepository = courseRepository;
        }

        public async Task CreateFavorite(FavoriteRequest request)
        {
            var userId = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userId == null)
            {
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }

            var course = await courseRepository.FindById(request.CourseId);
            if (course == null)
            {
                throw new AppException(ErrorCode.COURSE_NOT_EXISTED);
            }
            bool isAlreadyFavorite = await favoriteRepository.ExistsByUserAndCourse(int.Parse(userId.Value), course.Id);

            if (isAlreadyFavorite)
            {
                throw new AppException(ErrorCode.FAVORITE_EXISTED);
            }
            Favorite favorite = new Favorite
            {
                CourseId = course.Id,
                UserId = int.Parse(userId.Value)
            };
            await favoriteRepository.AddFavorite(favorite);
        }

        public async Task<PageResponse<FavoriteResponse>> GetFavoriteCourses(int page, int size)
        {
            var userId = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userId == null)
            {
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }

            var skip = (page - 1) * size;

            var favorites = await favoriteRepository.GetFavoriteCourses(int.Parse(userId.Value), page, size);

            var totalElemets = await favoriteRepository.GetTotalFavoriteCoursesCount(int.Parse(userId.Value));

            var totalPages = (int)Math.Ceiling(totalElemets / (double)size);

            var favoriteResponses = favorites.Select(favorite => new FavoriteResponse
            {
                FavoriteId = favorite.Id,
                CourseId = favorite.CourseId,
                Title = favorite.Course.Title,
                Author = favorite.Course.Author.Name,
                Thumbnail = favorite.Course.Thumbnail,
                Price = favorite.Course.Price
            }).ToList();

            Console.WriteLine("FavoriteResponse: ", favoriteResponses);
            Console.WriteLine(totalPages);
            Console.WriteLine(totalElemets);

            return new PageResponse<FavoriteResponse>
            {
                CurrentPage = page,
                PageSize = size,
                TotalPages = totalPages,
                TotalElemets = totalElemets,
                Data = favoriteResponses
            };
        }

        public async Task RemoveFavorite(long favorite)
        {
            var userId = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userId == null)
            {
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }
            await favoriteRepository.RemoveFavorite(favorite);
        }
    }
}
