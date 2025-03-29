using E_Learning.Data;
using E_Learning.Entity;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Repositories
{
    public class PostRepository
    {

        private readonly ELearningDbContext _context;

        public PostRepository(ELearningDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Post>> GetAll()
        {
            return await _context.Posts.ToListAsync();
        }

        public async Task<Post> GetById(int id)
        {
            return await _context.Posts.FindAsync(id);
        }

        public async Task<List<Post>> FindAll(int page, int size)
        {
            if (page < 1)
            {
                page = 1;
            }
            return await _context.Posts
                .Include(p => p.User)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();
        }

        public async Task<int> CountAllPost()
        {
            return await _context.Posts.CountAsync();
        }

        public async Task Create(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Post post)
        {
            _context.Entry(post).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

    }
}
