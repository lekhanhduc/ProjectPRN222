namespace E_Learning.Dto.Response.admin
{
    public class TeacherApplicationDetailResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public string? Avatar { get; set; }
        public DateOnly Dob { get; set; }
        public string? CvUrl { get; set; }
        public string? Certificate { get; set; }
        public string? FacebookLink { get; set; }
        public string? Description { get; set; }
        public double? YearsOfExperience { get; set; }
        public long Points { get; set; }
        public string Role { get; set; }
    }

}
