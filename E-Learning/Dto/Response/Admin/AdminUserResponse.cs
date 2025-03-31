namespace E_Learning.Dto.Response.admin
{
    public class AdminUserResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public bool Enabled { get; set; }
        public string Role { get; set; }
        public DateTime CreateAt { get; set; }
    }

}