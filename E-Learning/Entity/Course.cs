using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Learning.Entity
{
    [Table("courses")]
    public class Course : BaseEntity<long>
    {

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
        public string Level { get; set; }

        [Column("language")]
        public string Language { get; set; }

        [Column("price", TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; } = 0;

        [ForeignKey("Author")]
        [Column("user_id")]
        public long AuthorId { get; set; }
        public User Author { get; set; }

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
        public ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();

    }
}
