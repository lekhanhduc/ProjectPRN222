using E_Learning.Dto.Request;
using E_Learning.Dto.Response;

namespace E_Learning.Servies
{
    public interface IFavoriteService
    {
        Task CreateFavorite(FavoriteRequest request);
        Task<PageResponse<FavoriteResponse>> GetFavoriteCourses(int page, int size);
        Task RemoveFavorite(long favorite);
    }
}
