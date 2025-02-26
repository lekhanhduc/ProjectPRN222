using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Learning.Entity
{
    [Table("users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [EmailAddress]
        [Column("email")]
        public string Email { get; set; }

        [Column("password")]
        public string? Password { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Required]
        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("refresh_token")]
        public string? RefreshToken { get; set; }

        [Required]
        [Column("last_name")]
        public string LastName { get; set; }

        [Column("avatar")]
        public string? Avatar { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("dob")]
        public DateOnly Dob { get; set; }

        [Column("enabled")]
        public bool Enabled { get; set; }

        public ICollection<Course> Courses { get; set; }

        [Required]
        [ForeignKey("Role")]
        public int RoleId { get; set; }


        public Role Role { get; set; }

    }
}
