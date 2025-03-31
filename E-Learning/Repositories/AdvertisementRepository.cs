using Azure;
using E_Learning.Common;
using E_Learning.Data;
using E_Learning.Dto.Response.Admin;
using E_Learning.Entity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

        // Lấy tất cả quảng cáo với phân trang
        public async Task<List<Advertisement>> GetAll(int skip, int take)
        {
            return await _context.Advertisements
                                 .Skip(skip)  // Bỏ qua số bản ghi tính từ skip
                                 .Take(take)  // Lấy số bản ghi theo số lượng (take)
                                 .ToListAsync();
        }

        // Lấy tổng số phần tử trong bảng
        public async Task<int> GetTotalCount()
        {
            return await _context.Advertisements.CountAsync();
        }

        public async Task<Advertisement> GetByIdAsync(long id)
        {
            return await _context.Advertisements
                .FirstOrDefaultAsync(a => a.Id == id);
        }
        public async Task SaveAsync(Advertisement advertisement)
        {
            _context.Advertisements.Update(advertisement);
            await _context.SaveChangesAsync();
        }


    }
}
