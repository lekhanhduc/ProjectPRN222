using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using E_Learning.Common;
using E_Learning.Data;
using E_Learning.Dto.Request;
using E_Learning.Dto.Response;
using E_Learning.Entity;
using E_Learning.Middlewares;
using E_Learning.Models.Request;
using E_Learning.Models.Response;
using E_Learning.Repositories;
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
        private readonly GoogleAuthClient googleAuthClient;
        private readonly GoogleUserInfoClient googleUserInfoClient;
        private readonly RoleRepository roleRepository;

        public AuthenticationService(
            ELearningDbContext context,
            IConfiguration configuration,
            ILogger<AuthenticationService> logger,
            IJwtService jwtService,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration _configuration,
            GoogleUserInfoClient googleUserInfoClient,
            GoogleAuthClient googleAuthClient,
            RoleRepository roleRepository)
        {
            this.context = context;
            this.configuration = configuration;
            this.logger = logger;
            this.jwtService = jwtService;
            this.passwordHasher = new PasswordHasher<User>();
            this._httpContextAccessor = httpContextAccessor;
            this._configuration = _configuration;
            this.googleUserInfoClient = googleUserInfoClient;
            this.googleAuthClient = googleAuthClient;
            this.roleRepository = roleRepository;
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
                    SameSite = SameSiteMode.Lax,
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

        public async Task<IntrospectResponse> VerifyRefreshToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key-Refresh-Token"]);

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
                    ClockSkew = TimeSpan.Zero // Đảm bảo thời gian hết hạn chính xác
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                var roleClaim = principal?.FindFirst("Authorities")?.Value;

                return new IntrospectResponse
                {
                    Valid = true,
                    Scope = roleClaim ?? "Unknown"
                };
            }
            catch (Exception ex)
            {
                return new IntrospectResponse
                {
                    Valid = false,
                    Scope = "Invalid"
                };
            }
        }

        public async Task<SignInResponse> RefreshToken()
        {
            logger.LogInformation("RefreshToken start ...");
            var tokenHandler = new JwtSecurityTokenHandler();
            var refreshToken = _httpContextAccessor.HttpContext?.Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new AppException(ErrorCode.REFRESH_TOKEN_MISSING);
            }

            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key-Refresh-Token"]);

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

            try
            {
                var principal = tokenHandler.ValidateToken(refreshToken, validationParameters, out _);

                var userId = principal?.FindFirst("userId")?.Value;
                var user = await context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id.ToString() == userId);

                if (user == null)
                {
                    throw new AppException(ErrorCode.USER_NOT_EXISTED);
                }

                var claims = new[]
                {
            new Claim("userId", user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("Authorities", user.Role.Name)
        };
                var newAccessToken = jwtService.GenerateAccessToken(claims);

                var httpContext = _httpContextAccessor?.HttpContext;
                if (httpContext != null)
                {
                    httpContext.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = false, // Cập nhật thành true nếu sử dụng HTTPS
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddDays(14)
                    });
                }

                await context.SaveChangesAsync();
                logger.LogInformation("RefreshToken Success ...");
                return new SignInResponse(newAccessToken, refreshToken, user.Role.Name);
            }
            catch (Exception ex)
            {
                throw new AppException(ErrorCode.INVALID_REFRESH_TOKEN);
            }
        }

        public async Task SignOut()
        {
            logger.LogInformation("SignOut start ...");

            var httpContext = _httpContextAccessor?.HttpContext;
            if (httpContext != null)
            {
                httpContext.Response.Cookies.Delete("refreshToken");
            }

            var userId = _httpContextAccessor?.HttpContext?.User?.FindFirst("userId")?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                var user = await context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
                if (user != null)
                {
                    user.RefreshToken = null;
                    await context.SaveChangesAsync();
                }
                else
                {
                    logger.LogWarning("SignOut failed: User not found in database.");
                }
            }
            else
            {
                logger.LogWarning("SignOut failed: No userId in context.");
            }

            logger.LogInformation("SignOut success.");
        }

        public async Task<SignInResponse> SignInWithGoogle(string code)
        {
            
            var token = await googleAuthClient.ExchangeTokenAsync(code);
            var userInfo = await googleUserInfoClient.GetUserInfoAsync(token.AccessToken);

            var user =  await context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == userInfo.Email);

            if (user == null)
            {
                var role = await roleRepository.FindByRoleName(DefinitionRole.USER);
                if (role == null)
                {
                    role = new Role();
                    role.Name = DefinitionRole.USER;
                    await roleRepository.CreateRole(role);
                }
                user = new User
                {
                    Email = userInfo.Email,
                    FirstName = userInfo.GivenName,
                    LastName = userInfo.FamilyName,
                    Name = userInfo.Name,
                    Avatar = userInfo.Picture,
                    Enabled = true,
                    Role = role
                };
                context.Users.Add(user);
                await context.SaveChangesAsync();
            }
            var claims = new[]
    {
        new Claim("userId", user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("Authorities", user.Role.Name)
    };

            var accessToken = jwtService.GenerateAccessToken(claims);
            var refreshToken = jwtService.GenerateRefreshToken(claims);
            Console.WriteLine(refreshToken);

            var httpContext = _httpContextAccessor?.HttpContext;
            if (httpContext != null)
            {
                httpContext.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false,
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTimeOffset.UtcNow.AddDays(14)
                });
            }

            user.RefreshToken = refreshToken;
            await context.SaveChangesAsync();

            logger.LogInformation("SignIn Google success for userId: {UserId}", user.Id);

            return new SignInResponse(accessToken, refreshToken, user.Role.Name);
        }
    }
}
