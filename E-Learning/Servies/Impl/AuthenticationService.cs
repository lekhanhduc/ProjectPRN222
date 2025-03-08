using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using E_Learning.Data;
using E_Learning.Dto.Request;
using E_Learning.Dto.Response;
using E_Learning.Entity;
using E_Learning.Middlewares;
using E_Learning.Models.Request;
using E_Learning.Models.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace E_Learning.Servies.Impl
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ELearningDbContext context;
        private readonly IConfiguration configuration;
        private readonly ILogger<AuthenticationService> logger;
        private readonly IJwtService jwtService;
        private readonly PasswordHasher<User> passwordHasher;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public AuthenticationService(
            ELearningDbContext context,
            IConfiguration configuration,
            ILogger<AuthenticationService> logger,
            IJwtService jwtService,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration _configuration)
        {
            this.context = context;
            this.configuration = configuration;
            this.logger = logger;
            this.jwtService = jwtService;
            this.passwordHasher = new PasswordHasher<User>();
            this._httpContextAccessor = httpContextAccessor;
            this._configuration = _configuration;
        }

        public async Task<SignInResponse> SignIn(SignInRequest request)
        {
            logger.LogInformation("SignIn start ...");

            var user = await context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                logger.LogError("SignIn Failed: User not found.");
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }

            if (!user.Enabled)
            {
                throw new AppException(ErrorCode.ACCOUNT_LOCKED);
            }

            var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.Password, request.Password);

            if (passwordVerificationResult != PasswordVerificationResult.Success)
            {
                logger.LogError("SignIn Failed: Invalid password.");
                throw new AppException(ErrorCode.UNAUTHORIZED);
            }

            var claims = new[]
    {
        new Claim("userId", user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("Authorities", user.Role.Name)
    };

            var accessToken = jwtService.GenerateAccessToken(claims);
            var refreshToken = jwtService.GenerateRefreshToken(claims);

            // Kiểm tra HttpContext trước khi đặt cookie
            var httpContext = _httpContextAccessor?.HttpContext;
            if (httpContext != null)
            {
                httpContext.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddDays(14)
                });
            }

            user.RefreshToken = refreshToken;
            await context.SaveChangesAsync();

            logger.LogInformation("SignIn success for userId: {UserId}", user.Id);

            return new SignInResponse(accessToken, refreshToken, user.Role.Name);
        }

        public async Task<IntrospectResponse> VerifyToken(IntrospectRequest request)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(request.Token, validationParameters, out _);
                var roleClaim = principal?.FindFirst("Authorities")?.Value;

                return new IntrospectResponse
                {
                    Valid = true,
                    Scope = roleClaim ?? "Unknown"
                };
            }
            catch
            {
                return new IntrospectResponse
                {
                    Valid = false,
                    Scope = "Invalid"
                };
            }
        }

    }
}
