using E_Learning.Common;
using E_Learning.Data;
using E_Learning.Entity;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<Advertisement>> FindAdvertisementByApprovalStatusActive(AdsStatus status)
        {
            return await _context.Advertisements
                .Where(a => a.ApprovalStatus == status)
                .ToListAsync();
        }

    }
}
