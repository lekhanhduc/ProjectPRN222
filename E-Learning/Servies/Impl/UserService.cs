using E_Learning.Common;
using E_Learning.Dto.Response;
using E_Learning.Entity;
using E_Learning.Middlewares;
using E_Learning.Models.Request;
using E_Learning.Models.Response;
using E_Learning.Repositories;
using E_Learning.Utils;
using Microsoft.AspNetCore.Identity;

namespace E_Learning.Servies.Impl
{
    public class UserService : IUserService
    {

        private readonly UserRepository userRepository;
        private readonly RoleRepository roleRepository;
        private readonly PasswordHasher<User> passwordHasher;
        private readonly ILogger<UserService> logger;
        private readonly IEmailService emailService;
        private readonly IRedisService redisService;

        public UserService(UserRepository userRepository,
                           RoleRepository roleRepository,
                           ILogger<UserService> logger,
                           IEmailService emailService,
                           IRedisService redisService)
        {
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.passwordHasher = new PasswordHasher<User>();
            this.logger = logger;
            this.emailService = emailService;
            this.redisService = redisService;
        }

        public async Task<UserCreationResponse> CreateUser(UserCreationRequest request)
        {
            var userOptinal = await userRepository.FindUserByEmail(request.Email);
            if (userOptinal != null)
            {
                logger.LogError("User Existed");
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }

            User user = new User();
            user.Email = request.Email;
            user.Password = passwordHasher.HashPassword(user, request.Password.Trim());
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Name = request.FirstName + " " + request.LastName;
            user.Dob = request.Dob;
            user.Enabled = false;

            var role = await roleRepository.FindByRoleName(DefinitionRole.USER);
            if (role == null)
            {
                role = new Role();
                role.Name = DefinitionRole.USER;
                await roleRepository.CreateRole(role);
            }
            user.Role = role;
            await userRepository.CreateUserAsync(user);

            logger.LogInformation("User created successfully with email: {Email}", request.Email);
            var otp = GenerateOtp.Generate();
            var verificationLink = $"https://localhost:7207/api/users/verification?email={user.Email}&otp={otp}";

            redisService.SaveOtp(user.Email, otp, TimeSpan.FromMinutes(5));

            await emailService.SendEmailVerification(user.Email, user.Name, verificationLink, otp);

            return new UserCreationResponse
                (
                    user.Email,
                    user.FirstName,
                    user.LastName,
                    user.Dob,
                    user.Role.Name
                );
        }

        public async Task<IEnumerable<UserResponse>> FindAll()
        {
            var users = await userRepository.GetAllUsers();

            var result = users.Select(user => new UserResponse
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Name = user.Name,
            });

            return result;
        }

        public async Task<VerificationResponse> Verification(string email, string otp)
        {
            var user = await userRepository.FindUserByEmail(email);
            if (user == null)
            {
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }
            var otpRedis = redisService.GetOtp(user.Email);

            if (!string.Equals(otp, otpRedis, StringComparison.Ordinal))
            {
                throw new AppException(ErrorCode.OTP_INVALID);
            }

            redisService.DeleteOtp(user.Email);
            user.Enabled = true;
            await userRepository.UpdateUserAsync(user);

            logger.LogInformation("User verified successfully: {Email}", user.Email);

            return new VerificationResponse
                (
                    Success: true
                );
        }

    }
}
