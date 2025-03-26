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

        public ProfileService(UserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            this.userRepository = userRepository;
            this.httpContextAccessor = httpContextAccessor;
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
