using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Learning.Entity
{
    [Table("lessons_progress")]
    public class LessonProgress : BaseEntity<long>
    {
        [ForeignKey("UserId")]
        public User User { get; set; }
        public long UserId { get; set; }

        [ForeignKey("LessonId")]
        public virtual Lesson Lesson { get; set; }
        public long LessonId { get; set; }

        [Required]
        [Column("completed")]
        public bool Completed { get; set; } = false;
    }
}
