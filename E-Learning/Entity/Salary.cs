using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using E_Learning.Common;

namespace E_Learning.Entity
{
    public class Salary
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [ForeignKey("Author")]
        public long? AuthorId { get; set; }
        public User? Author { get; set; }


        [Required]
        [Column("money", TypeName = "decimal(18,2)")]
        public decimal Money { get; set; }

        [Required]
        [Column("status", TypeName = "VARCHAR(20)")] // Đủ chứa các giá trị enum
        public TransactionStatus Status { get; set; }

        [Column("created_by", TypeName = "VARCHAR(20)")]
        public string? CreatedBy { get; set; }

        [Column("updated_by", TypeName = "VARCHAR(20)")]
        public string? UpdatedBy { get; set; }

        [Column("create_at", TypeName = "DATETIME")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("update_at", TypeName = "DATETIME")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    }
}
