using E_Learning.Common;

namespace E_Learning.Dto.Request
{
    public class UserProfileRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public string? Phone { get; set; }
        public DateOnly Dob { get; set; }
        public string? Address { get; set; }

        public string? Description { get; set; }
        public string? Level { get; set; }
    }

}
