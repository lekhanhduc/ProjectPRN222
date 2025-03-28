using E_Learning.Common;
using E_Learning.Dto.Request;
using E_Learning.Dto.Response;
using E_Learning.Middlewares;
using E_Learning.Repositories;

namespace E_Learning.Servies.Impl
{
    public class ProfileService: IProfileService
    {

        private readonly UserRepository userRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly CloudinaryService cloudinaryService;

        public ProfileService(UserRepository userRepository, IHttpContextAccessor httpContextAccessor, CloudinaryService cloudinaryService)
        {
            this.userRepository = userRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<string?> GetAvatar()
        {
            var userId = httpContextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(c => c.Type == "userId");

            if (userId == null)
            {
                return null;
            }

            var user = await userRepository.FindUserById(int.Parse(userId.Value));

            if (user == null)
            {
                return null;
            }

            return user.Avatar; 
        }


        public async Task<UserProfileResponse> GetUserProfile()
        {
            var userId = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userId == null)
            {
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }
            var user = await userRepository.FindUserById(int.Parse(userId.Value));
            if (user == null)
            {
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }
            return new UserProfileResponse
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                Description = user.Description,
                Avatar = user.Avatar,
                Dob = user.Dob,
                Gender = user.Gender,
                Level = user.Level,
                Phone = user.Phone,
            };
        }

        public async Task RomoveAvatar()
        {

            var userId = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userId == null)
            {
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }
            var user = await userRepository.FindUserById(int.Parse(userId.Value));
            if (user == null)
            {
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }
            user.Avatar = null;
            await userRepository.UpdateUserAsync(user);
        }

        public async Task<string?> UpdateAvatar(UpdateAvatarRequest request)
        {
            var userId = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userId == null)
            {
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }
            var user = await userRepository.FindUserById(int.Parse(userId.Value));
            if (user == null)
            {
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }
            using var stream = request.File.OpenReadStream();
            var imageUrl = await cloudinaryService.UploadImageAsync(stream, request.File.Name);
            user.Avatar = imageUrl;
            await userRepository.UpdateUserAsync(user);
            return imageUrl;
        }

        public async Task UpdateUserProfile(UserProfileRequest request)
        {
            var userId = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userId == null)
            {
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }
            var user = await userRepository.FindUserById(int.Parse(userId.Value));
            if (user == null)
            {
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }

            if(request.FirstName != null)
            {
                user.FirstName = request.FirstName;
            }
            if (request.LastName != null)
            {
                user.LastName = request.LastName;
            }
            if (request.Address != null)
            {
                user.Address = request.Address;
            }
            if (request.Description != null)
            {
                user.Description = request.Description;
            }
            if (request.Dob != null)
            {
                user.Dob = request.Dob;
            }
            if (Enum.TryParse(request.Gender, true, out Gender gender))
            {
                user.Gender = gender;
            }
            if (request.Level != null)
            {
                user.Level = request.Level;
            } 
            if(request.Phone != null)
            {
                user.Phone = request.Phone;
            }
            await userRepository.UpdateUserAsync(user);
        }
    }
}
