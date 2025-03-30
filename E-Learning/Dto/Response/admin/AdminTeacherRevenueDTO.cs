namespace E_Learning.Dto.Response.admin
{
    public class AdminTeacherRevenueDTO
    {
        public long TeacherId { get; set; }
        public string TeacherName { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalCoursesSold { get; set; }
        public string TeacherAvatar { get; set; }
    }
}
