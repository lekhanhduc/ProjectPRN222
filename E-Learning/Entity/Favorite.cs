using System.ComponentModel.DataAnnotations.Schema;

namespace E_Learning.Entity
{
    [Table("favorites")]
    public class Favorite : BaseEntity<long>
    {
        [ForeignKey("UserId")]
        public User User { get; set; }
        public long UserId { get; set; }

        [ForeignKey("CourseId")]
        public Course Course { get; set; }
        public long CourseId { get; set; }
    }
}
