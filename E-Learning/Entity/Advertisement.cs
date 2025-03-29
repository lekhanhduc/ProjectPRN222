using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using E_Learning.Common;

namespace E_Learning.Entity
{
    [Table("advertisements")]
    public class Advertisement : BaseEntity<long>
    {
        [Required]
        [MaxLength(100)]
        [Column("title")]
        public string Title { get; set; }

        [Column("description", TypeName = "TEXT")]
        public string? Description { get; set; }

        [Required]
        [EmailAddress]
        [Column("contact_email")]
        public string ContactEmail { get; set; }

        [Required]
        [Column("contact_phone")]
        public string ContactPhone { get; set; }

        [Column("price", TypeName = "decimal(18,2)")]

        public decimal? Price { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }
        public long? UserId { get; set; }

        [ForeignKey("CourseId")]
        public Course? Course { get; set; }
        public long? CourseId { get; set; }


        [Column("image_ads")]
        public string? Image { get; set; }


        [Column("location")]
        public string? Location { get; set; }


        [Column("link")]
        public string? Link { get; set; }

        [Column("start_date")]
        public DateOnly StartDate { get; set; }

        [Column("end_date")]
        public DateOnly EndDate { get; set; }

        [Required]
        [Column("approval_status", TypeName = "nvarchar(50)")]
        public AdsStatus ApprovalStatus { get; set; }
    }
}
