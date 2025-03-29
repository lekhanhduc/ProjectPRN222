using E_Learning.Common;
using E_Learning.Dto.Request;
using E_Learning.Dto.Response;
using E_Learning.Entity;
using E_Learning.Middlewares;
using E_Learning.Repositories;

namespace E_Learning.Servies.Impl
{
    public class AdvertisementService : IAdvertisementService
    {

        private readonly AdvertisementRepository advertisementRepository;
        private readonly UserRepository userRepository;
        private readonly CloudinaryService cloudinaryService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AdvertisementService(AdvertisementRepository advertisementRepository, UserRepository userRepository, CloudinaryService cloudinaryService, IHttpContextAccessor httpContextAccessor)
        {
            this.advertisementRepository = advertisementRepository;
            this.userRepository = userRepository;
            this.cloudinaryService = cloudinaryService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<AdsCreationResponse> CreateAdsAsync(AdsCreationRequest request)
        {
            var userIdClaim = httpContextAccessor.HttpContext?.User.Claims
              .FirstOrDefault(c => c.Type == "userId");

            if (userIdClaim == null)
            {
                throw new AppException(ErrorCode.UNAUTHORIZED);
            }

            var user = await userRepository.FindUserById(long.Parse(userIdClaim.Value));

            if (user == null)
            {
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }

            using var stream = request.Image.OpenReadStream();
            var imageUrl = await cloudinaryService.UploadImageAsync(stream, request.Image.Name);

            var advertisement = new Advertisement
            {
                Title = request.Title,
                Description = request.Description,
                ContactEmail = request.ContactEmail,
                ContactPhone = request.ContactPhone,
                Image = imageUrl,
                Location = request.Location,
                Link = request.Link,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                ApprovalStatus = AdsStatus.PENDING,
                UserId = user.Id
            };

            await advertisementRepository.CreateAsync(advertisement);

            var response = new AdsCreationResponse
            {
                Id = advertisement.Id,
                ContactEmail = advertisement.ContactEmail,
                ContactPhone = advertisement.ContactPhone,
                Title = advertisement.Title,
                Description = advertisement.Description,
                ImageUrl = advertisement.Image,
                Location = advertisement.Location,
                Link = advertisement.Link,
                StartDate = advertisement.StartDate,
                EndDate = advertisement.EndDate,
                PriceAds = advertisement.Price ?? 0,  
                Status = advertisement.ApprovalStatus,
                CreateAt = advertisement.CreatedAt
            };

            return response;
        }

        public async Task<List<AdsActiveResponse>> GetAdsWithActive()
        {
            var advertisements = await advertisementRepository.FindAdvertisementByApprovalStatusActive(AdsStatus.COMPLETED);

            return advertisements.Select(ads => new AdsActiveResponse
            {
                Id = ads.Id,
                Title = ads.Title,
                Image = ads.Image,
                Price = ads.Price ?? 0, 
                Description = ads.Description,
                Link = ads.Link,
                StartDate = ads.StartDate,
                EndDate = ads.EndDate
            }).ToList();
        }

    }
}
