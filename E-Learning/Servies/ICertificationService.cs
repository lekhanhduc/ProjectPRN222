using E_Learning.Dto.Event;
using E_Learning.Dto.Response;

namespace E_Learning.Servies
{
    public interface ICertificationService
    {
        Task CreateCrertification(CertificateCreationEvent creationEvent);
        Task<List<CertificateResponse>> GetCertificateByUserLogin();
    }
}
