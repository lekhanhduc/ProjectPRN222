using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Learning.Entity
{
    public abstract class BaseEntity<T>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public T Id { get; set; }

        [Column("created_by")]
<<<<<<< HEAD
        public string? CreatedBy { get; set; }

        [Column("updated_by")]
        public string? UpdatedBy { get; set; }
=======
        public string CreatedBy { get; set; }

        [Column("updated_by")]
        public string UpdatedBy { get; set; }
>>>>>>> 340b601 (fix conflict when rebase)

        [Column("create_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("update_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
<<<<<<< HEAD

=======
>>>>>>> 340b601 (fix conflict when rebase)
}
