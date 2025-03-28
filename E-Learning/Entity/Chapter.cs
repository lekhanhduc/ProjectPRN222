using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Learning.Entity
{
    [Table("chapters")]
    public class Chapter : BaseEntity<long>
    {
        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }
        public long CourseId { get; set; }

        public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

        [Required]
        [Column("chapter_name")]
        public string ChapterName { get; set; }

        [Column("description", TypeName = "TEXT")]
        public string? Description { get; set; }

    }
}
