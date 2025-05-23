﻿using E_Learning.Common;

namespace E_Learning.Dto.Response
{
    public class UserProfileResponse
    {
        public string? Avatar { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender? Gender { get; set; }
        public string? Phone { get; set; }
        public DateOnly? Dob { get; set; }
        public string? Address { get; set; }

        public string? Description { get; set; }
        public string? Level { get; set; }
    }
}
