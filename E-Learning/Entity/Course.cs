using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using E_Learning.Common;

namespace E_Learning.Entity
{
    [Table("courses")]
    public class Course
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("title")]
        public string Title { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("thumbnail")]
        public string Thumbnail { get; set; }

        [Column("duration")]
        public int Duration { get; set; }

        [Column("level")]
        public Level Level { get; set; }

        [Column("language")]
        public string Language { get; set; }

        [Column("point", TypeName = "decimal(18, 2)")]
        public decimal Point { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [ForeignKey("user_id")]
        public User Author { get; set; }
    }
}
