using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Learning.Entity
{
    [Table("comments")]
    public class Comment : BaseEntity<long>
    {
        [Required]
        [Column("content", TypeName = "TEXT")]
        public string Content { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public long UserId { get; set; }

        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }
        public long PostId { get; set; }

        // Quan hệ N-1 với Comment cha (Mỗi Comment có thể có một Comment cha)
        [ForeignKey("ParentCommentId")]
        public virtual Comment? ParentComment { get; set; }
        public long? ParentCommentId { get; set; }

        // Quan hệ 1-N với Comment con (Mỗi Comment có thể có nhiều Reply)
        public virtual ICollection<Comment> Replies { get; set; } = new List<Comment>();
    }
}
