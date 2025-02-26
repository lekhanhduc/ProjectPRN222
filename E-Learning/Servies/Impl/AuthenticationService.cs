using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using E_Learning.Data;
using E_Learning.Middlewares;
using E_Learning.Models.Request;
using E_Learning.Models.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Servies.Impl
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ELearningDbContext context;
        private readonly IConfiguration configuration;
        private readonly ILogger<AuthenticationService> logger;
        private readonly IJwtService jwtService;
        private readonly PasswordHasher<string> passwordHasher;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationService(
            ELearningDbContext context,
            IConfiguration configuration,
            ILogger<AuthenticationService> logger,
            IJwtService jwtService,
            IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this.configuration = configuration;
            this.logger = logger;
            this.jwtService = jwtService;
            this.passwordHasher = new PasswordHasher<string>();
            this._httpContextAccessor = httpContextAccessor;
        }

        public async Task<SignInResponse> SignIn(SignInRequest request)
        {
            logger.LogInformation("SignIn start ...");

            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                logger.LogError("SignIn Failed: User not found.");
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }

            if (user.Enabled == false)
            {
                throw new AppException(ErrorCode.ACCOUNT_LOCKED);
            }

            var passwordVerificationResult = passwordHasher.VerifyHashedPassword(null, user.Password, request.Password);

            if (passwordVerificationResult != PasswordVerificationResult.Success)
            {
                logger.LogError("SignIn Failed: Invalid password.");
                throw new AppException(ErrorCode.UNAUTHORIZED);
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var accessToken = jwtService.GenerateAccessToken(claims);
            var refreshToken = jwtService.GenerateRefreshToken(claims);

            // Sử dụng IHttpContextAccessor để set cookie
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                throw new InvalidOperationException("HttpContext is not available.");
            }

            httpContext.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true, // Chỉ cho phép truy cập từ server
                Secure = false,   // Chỉ gửi cookie qua HTTPS nếu true
                SameSite = SameSiteMode.Strict, // Ngăn chặn CSRF
                Expires = DateTimeOffset.UtcNow.AddDays(14) // Thời gian sống của cookie
            });

            user.RefreshToken = refreshToken;
            await context.SaveChangesAsync();

            logger.LogInformation("SignIn success for userId: {UserId}", user.Id);

            return new SignInResponse(accessToken, refreshToken, user.Id);
        }

    }
}
