using E_Learning.Data;
using E_Learning.Entity;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Repositories
{
    public class ChapterRepository
    {
        private readonly ELearningDbContext context;

        public ChapterRepository(ELearningDbContext context)
        {
            this.context = context;
        }

        public async Task<Chapter> FindById(long id)
        {
            return await context.Chapters.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Chapter> Create(Chapter chapter)
        {
            await context.Chapters.AddAsync(chapter);
            await context.SaveChangesAsync();
            return chapter;
        }

        public async Task<Chapter> Update(Chapter chapter)
        {
            context.Chapters.Update(chapter);
            await context.SaveChangesAsync();
            return chapter;
        }

        public async Task Delete(Chapter chapter)
        {
            context.Chapters.Remove(chapter);
            await context.SaveChangesAsync();
        }

        public async Task<ICollection<Chapter>> FindAll()
        {
            return await context.Chapters.ToListAsync();
        }
    }
}
