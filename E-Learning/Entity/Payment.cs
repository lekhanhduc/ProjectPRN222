using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using E_Learning.Common;

namespace E_Learning.Entity
{
    [Table("payments")]
    public class Payment : BaseEntity<long>
    {
        [ForeignKey("UserId")]
        public User? User { get; set; }
        public long? UserId { get; set; }

        [ForeignKey("CourseId")]
        public virtual Course? Course { get; set; }
        public long? CourseId { get; set; }

        [Column("order_code")]
        public long? OrderCode { get; set; }

        [Required]
        [Column("payment_method", TypeName = "nvarchar(50)")]
        public PaymentMethod PaymentMethod { get; set; }

        [Required]
        [Column("payment_gate_way", TypeName = "nvarchar(50)")]
        public PaymentGateWay PaymentGateWay { get; set; }

        [Required]
        [Column("currency", TypeName = "nvarchar(50)")]
        public Currency Currency { get; set; }

        [Required]
        [Column("status", TypeName = "nvarchar(50)")]
        public PaymentStatus Status { get; set; }

        [Required]
        [Column("price", TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
    }
}
