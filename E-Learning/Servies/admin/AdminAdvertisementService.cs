using AutoMapper;
using E_Learning.Common;
using E_Learning.Dto.Request;
using E_Learning.Dto.Response;
using E_Learning.Dto.Response.Admin;
using E_Learning.Middlewares;
using E_Learning.Repositories;

namespace E_Learning.Servies.admin
{
    public class AdminAdvertisementService : IAdminAdvertisementService
    {
        private readonly AdvertisementRepository _advertisementRepository;

        public AdminAdvertisementService(AdvertisementRepository advertisementRepository)
        {
            _advertisementRepository = advertisementRepository;
        }

        public async Task<PageResponse<AdsCreationResponse>> GetAllAds(int page, int size)
        {
            var skip = (page - 1) * size; // Tính toán số phần tử cần bỏ qua
            var advertisements = await _advertisementRepository.GetAll(skip, size);

            var adsResponses = advertisements.Select(ad => new AdsCreationResponse
            {
                Id = ad.Id,
                ContactEmail = ad.ContactEmail,
                ContactPhone = ad.ContactPhone,
                Title = ad.Title,
                Description = ad.Description,
                ImageUrl = ad.Image,
                Location = ad.Location,
                Link = ad.Link,
                StartDate = ad.StartDate,
                EndDate = ad.EndDate,
                PriceAds = ad.Price ?? 0,  // Nếu Price là null, gán 0
                Status = ad.ApprovalStatus,
                CreateAt = ad.CreatedAt
            }).ToList();

            var totalCount = await _advertisementRepository.GetTotalCount();

            return new PageResponse<AdsCreationResponse>
            {
                CurrentPage = page,
                PageSize = size,
                TotalPages = (int)Math.Ceiling((double)totalCount / size),
                TotalElemets = totalCount,
                Data = adsResponses
            };
        }

        public async Task<AdsApproveResponse> ApproveAds(AdsApproveRequest request)
        {
            var advertisement = await _advertisementRepository.GetByIdAsync(request.Id);
            if (advertisement == null)
            {
                throw new AppException("Advertisement not found.");
            }

            advertisement.ApprovalStatus = AdsStatus.AWAITING_PAYMENT;
            await _advertisementRepository.SaveAsync(advertisement);

            return new AdsApproveResponse
            {
                Id = advertisement.Id,
                ContactEmail = advertisement.ContactEmail,
                ContactPhone = advertisement.ContactPhone,
                Title = advertisement.Title,
                Description = advertisement.Description,
                Image = advertisement.Image,
                Link = advertisement.Link,
                StartDate = advertisement.StartDate,  
                EndDate = advertisement.EndDate,  
                PriceAds = advertisement.Price ?? 0,  
                Status = advertisement.ApprovalStatus
            };
        }

        public async Task<AdsApproveResponse> RejectAds(AdsApproveRequest request)
        {
            var advertisement = await _advertisementRepository.GetByIdAsync(request.Id);
            if (advertisement == null)
            {
                throw new AppException("Advertisement not found.");
            }

            advertisement.ApprovalStatus = AdsStatus.REJECTED;  
            await _advertisementRepository.SaveAsync(advertisement);  

            return new AdsApproveResponse
            {
                Id = advertisement.Id,
                ContactEmail = advertisement.ContactEmail,
                ContactPhone = advertisement.ContactPhone,
                Title = advertisement.Title,
                Description = advertisement.Description,
                Image = advertisement.Image,  
                Link = advertisement.Link,  
                StartDate = advertisement.StartDate,
                EndDate = advertisement.EndDate,
                PriceAds = advertisement.Price ?? 0,
                Status = advertisement.ApprovalStatus
            };
        }
    }
}
