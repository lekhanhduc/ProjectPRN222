using E_Learning.Dto.Request;
using E_Learning.Dto.Response;

namespace E_Learning.Servies
{
    public interface IAdvertisementService
    {
        Task<Dto.Response.AdsCreationResponse> CreateAdsAsync(AdsCreationRequest request);
        Task<List<AdsActiveResponse>> GetAdsWithActive();
    }
}
