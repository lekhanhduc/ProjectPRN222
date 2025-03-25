using E_Learning.Data;
using E_Learning.Entity;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Repositories
{
    public class FavoriteRepository
    {
        private readonly ELearningDbContext elearningDbContext;

        public FavoriteRepository(ELearningDbContext elearningDbContext)
        {
            this.elearningDbContext = elearningDbContext;
        }

        public async Task AddFavorite(Favorite favorite)
        {
            await elearningDbContext.Favorites.AddAsync(favorite);
            await elearningDbContext.SaveChangesAsync();
        }

        public async Task RemoveFavorite(long favoriteId)
        {
            var favorite = await elearningDbContext.Favorites.FindAsync(favoriteId);
            if (favorite != null)
            {
                elearningDbContext.Favorites.Remove(favorite);
                await elearningDbContext.SaveChangesAsync();
            }
        }

        public async Task<List<Favorite>> GetFavoriteCourses(int userId, int page, int size)
        {
            var skip = (page - 1) * size;

            var favorites = await elearningDbContext.Favorites
                .Where(f => f.UserId == userId)
                .Include(f => f.Course)
                .ThenInclude(c => c.Author)
                .Skip(skip)
                .Take(size)
                .ToListAsync();

            return favorites;
        }

        public async Task<int> GetTotalFavoriteCoursesCount(int userId)
        {
            return await elearningDbContext.Favorites
                .Where(f => f.UserId == userId)
                .CountAsync();
        }


        public async Task<bool> ExistsByUserAndCourse(int userId, long courseId)
        {
            return await elearningDbContext.Favorites
                .AnyAsync(f => f.UserId == userId && f.CourseId == courseId);
        }

    }
}
