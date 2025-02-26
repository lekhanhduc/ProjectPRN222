using System.Security.Claims;

namespace E_Learning.Servies
{
    public interface IJwtService
    {
        string GenerateAccessToken(Claim[] claims);
        string GenerateRefreshToken(Claim[] claims);
    }
}
