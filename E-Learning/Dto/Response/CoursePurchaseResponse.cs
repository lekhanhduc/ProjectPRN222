namespace E_Learning.Dto.Response
{
    public class CoursePurchaseResponse
    {
        public long CourseId { get; set; }
        public long UserId { get; set; }
        public bool Purchased { get; set; }
    }
}
