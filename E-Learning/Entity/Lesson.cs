using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Learning.Entity
{
    [Table("lessons")]
    public class Lesson : BaseEntity<long>
    {

        [Required]
        [Column("lesson_name")]
        public string LessonName { get; set; }

        [ForeignKey("ChapterId")]
        public Chapter Chapter { get; set; }
        public long ChapterId { get; set; }

        [Column("content_type")]
        public string ContentType { get; set; } // "video", "document", etc.

        [Column("content_url")]
        public string? VideoUrl { get; set; }

        [Column("description", TypeName = "TEXT")]
        public string? Description { get; set; }

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<LessonProgress> LessonProgresses { get; set; } = new List<LessonProgress>();


    }
}
