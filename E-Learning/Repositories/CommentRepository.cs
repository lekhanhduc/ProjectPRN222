using E_Learning.Data;
using E_Learning.Entity;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Repositories
{
    public class CommentRepository
    {
        private readonly ELearningDbContext _context;
        public CommentRepository(ELearningDbContext context)
        {
            _context = context;
        }
        public async Task<Comment> FindById(long id)
        {
            return await _context.Comments
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task Save(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
        }
        public async Task<Comment> UpdateCommentAsync(Comment comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
            return comment;
        }
        public async Task Delete(Comment comment)
        {
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Comment>> FindCommentByPostIdAndParentCommentIsNull(long postId, int page, int size)
        {
            page = page < 1 ? 1 : page;

            int skip = (page - 1) * size;

            var comments = await _context.Comments
                .Where(c => c.PostId == postId && c.ParentCommentId == null)
                .Include(c => c.User)
                .Include(c => c.Replies)
                .ThenInclude(r => r.User)
                .OrderByDescending(c => c.CreatedAt) // Sort by CreatedAt descending (newest to oldest)
                .Skip(skip)
                .Take(size)
                .ToListAsync();

            return comments;
        }

        public async Task<int> CountCommentsByPostIdAndParentCommentIsNull(long postId)
        {
            return await _context.Comments
                .Where(c => c.PostId == postId && c.ParentCommentId == null)
                .CountAsync();
        }

    }
}
