﻿using E_Learning.Dto.Request;
using E_Learning.Dto.Response;
using E_Learning.Models.Response;
using E_Learning.Servies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.Controllers
{
    [Route("api/v1/profiles")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService profileService;
        public ProfileController(IProfileService profileService)
        {
            this.profileService = profileService;
        }

        [HttpGet]
        public async Task<ApiResponse<UserProfileResponse>> GetUserProfile()
        {
            var result = await profileService.GetUserProfile();
            return new ApiResponse<UserProfileResponse>
            {
                code = 200,
                message = "User Profile Response",
                result = result
            };
        }

        [HttpPut]
        public async Task<ApiResponse<object>> UpdateUserProfile([FromBody] UserProfileRequest request)
        {
            await profileService.UpdateUserProfile(request);
            return new  ApiResponse<object> {
                code = 201,
                message = "Update User Profile Successfully",
            };
        }

        [HttpGet("avatar")]
        public async Task<ApiResponse<string?>> GetAvatar()
        {
            var result = await profileService.GetAvatar();
            return new ApiResponse<string?>
            {
                code = 200,
                message = "Get Avatar Successfully",
                result = result
            };
        }

        [HttpPost("avatar")]
        public async Task<ApiResponse<string?>> UpdateAvatar([FromForm] UpdateAvatarRequest request)
        {
            var result = await profileService.UpdateAvatar(request);
            return new ApiResponse<string?>
            {
                code = 201,
                message = "Update Avatar Successfully",
                result = result
            };
        }

        [HttpDelete("avatar")]
        public async Task<ApiResponse<object>> RemoveAvatar()
        {
            await profileService.RomoveAvatar();
            return new ApiResponse<object>
            {
                code = 200,
                message = "Remove Avatar Successfully"
            };
        }

    }
}
