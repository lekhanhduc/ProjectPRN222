using E_Learning.Dto.Event;
using E_Learning.Dto.Response;
using E_Learning.Models.Response;
using E_Learning.Servies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.Controllers
{
    [Route("api/v1/certificate")]
    [ApiController]
    public class CertificationController : ControllerBase
    {
        private readonly ICertificationService certificationService;
        public CertificationController(ICertificationService certificationService)
        {
            this.certificationService = certificationService;
        }

        [HttpPost]
        public async Task<ApiResponse<object>> CreateCertificate(CertificateCreationEvent creationEvent)
        {
            await certificationService.CreateCrertification(creationEvent);
            return new ApiResponse<object>
            {
                code = 201,
                message = "Certificate created successfully"
            };
        }

        [HttpGet]
        public async Task<ApiResponse<List<CertificateResponse>>> GetCertificateByUserLogin()
        {
            var result = await certificationService.GetCertificateByUserLogin();
            return new ApiResponse<List<CertificateResponse>>
            {
                code = 200,
                message = "Get certificate successfully",
                result = result
            };
        }

    }
}
