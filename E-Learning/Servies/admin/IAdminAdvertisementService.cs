using E_Learning.Dto.Request;
using E_Learning.Dto.Response;
using E_Learning.Dto.Response.Admin;

namespace E_Learning.Servies.admin
{
    public interface IAdminAdvertisementService
    {
        Task<PageResponse<AdsCreationResponse>> GetAllAds(int page, int size);

        Task<AdsApproveResponse> ApproveAds(AdsApproveRequest request);

        Task<AdsApproveResponse> RejectAds(AdsApproveRequest request);

    }
}
