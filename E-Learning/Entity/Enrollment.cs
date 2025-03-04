using System.ComponentModel.DataAnnotations.Schema;

namespace E_Learning.Entity
{
    [Table("enrollments")]
    public class Enrollment : BaseEntity<long>
    {
        [ForeignKey("UserId")]
        public User? User { get; set; }
        public long UserId { get; set; }

        [ForeignKey("CourseId")]
        public Course? Course { get; set; }
        public long CourseId { get; set; }

        [Column("is_purchased")]
        public bool Purchased { get; set; } = true;

        [Column("is_complete")]
        public bool IsComplete { get; set; } = false;
    }
}
