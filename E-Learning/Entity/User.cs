using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Learning.Entity
{
    [Table("users")]
    public class User : BaseEntity<long>
    {
        [Required, EmailAddress]
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

        [Required]
        [Column("last_name")]
        public string LastName { get; set; }

        [Column("refresh_token")]
        public string? RefreshToken { get; set; }

        [Column("avatar")]
        public string? Avatar { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("dob")]
        public DateOnly Dob { get; set; }

        [Column("enabled")]
        public bool Enabled { get; set; } = true;

        [Required]
        [ForeignKey("Role")]
        [Column("role_id")]
        public int RoleId { get; set; }
        public Role Role { get; set; }

        public ICollection<Course> Courses { get; set; } = new List<Course>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
        public ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<LessonProgress> LessonProgresses { get; set; } = new List<LessonProgress>();
<<<<<<< HEAD

=======
>>>>>>> 340b601 (fix conflict when rebase)
    }
}
