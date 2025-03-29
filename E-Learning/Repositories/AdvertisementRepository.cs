using E_Learning.Data;
using E_Learning.Entity;

namespace E_Learning.Repositories
{
    public class AdvertisementRepository
    {

        private readonly ELearningDbContext _context;

        public AdvertisementRepository(ELearningDbContext context)
        {
            _context = context;
        }

        public async Task<Advertisement> CreateAsync(Advertisement advertisement)
        {
            await _context.Advertisements.AddAsync(advertisement);
            await _context.SaveChangesAsync();
            return advertisement;
        }
        

    }
}
