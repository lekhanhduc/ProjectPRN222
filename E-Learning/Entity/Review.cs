using System.ComponentModel.DataAnnotations.Schema;

namespace E_Learning.Entity
{
    [Table("reviews")]
    public class Review : BaseEntity<long>
    {
        public string? Content { get; set; }

        public int? Rating { get; set; }


        [ForeignKey("ParentReviewId")]
        [InverseProperty("Replies")]
        public Review? ParentReview { get; set; }
        public long? ParentReviewId { get; set; }

        public ICollection<Review>? Replies { get; set; } = new List<Review>();

        [ForeignKey("UserId")]
        public User User { get; set; }
        public long UserId { get; set; }


        [ForeignKey("CourseId")]
        public Course? Course { get; set; }
        public long? CourseId { get; set; }


        [ForeignKey("ChapterId")]
        public virtual Chapter? Chapter { get; set; }
        public long? ChapterId { get; set; }


        [ForeignKey("LessonId")]
        public Lesson? Lesson { get; set; }
        public long? LessonId { get; set; }

        public Review() { }

    }
}
