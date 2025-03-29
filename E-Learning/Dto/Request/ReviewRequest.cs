using EllipticCurve.Utils;
using System.Runtime.InteropServices;

namespace E_Learning.Dto.Request
{
    public class ReviewRequest
    {
        public string? Content { get; set; }  
        public long? ParentReviewId { get; set; } 
        public long CourseId { get; set; }  
        public int? Rating { get; set; }  
    }
}
