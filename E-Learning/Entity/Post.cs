using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Learning.Entity
{
    [Table("posts")]
    public class Post : BaseEntity<long>
    {
        [Required]
        [Column("title")]
        public string Title { get; set; }

        [Required]
        [Column("content", TypeName = "TEXT")]
        public string Content { get; set; }

        [Column("image")]
        public string? Image { get; set; }

        [Column("like_count")]
        public int LikeCount { get; set; } = 0; // Mặc định là 0

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public long UserId { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    }
}
