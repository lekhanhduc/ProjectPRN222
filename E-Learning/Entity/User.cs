using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Confluent.Kafka;
using E_Learning.Common;
using Microsoft.EntityFrameworkCore;

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

        [Column("gender", TypeName = "nvarchar(50)")]
        public Gender? Gender { get; set; }

        [Column("phone")]
        public string? Phone { get; set; }

        [Column("address")]
        public string? Address { get; set; }

        [Column("refresh_token")]
        public string? RefreshToken { get; set; }

        [Column("avatar")]
        public string? Avatar { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("dob")]
        public DateOnly Dob { get; set; }

        [Column("level")]
        public string? Level { get; set; }

        [Column("enabled")]
        public bool Enabled { get; set; } = true;

        [Required]
        [ForeignKey("Role")]
        [Column("role_id")]
        public int RoleId { get; set; }
        public Role Role { get; set; }

        [Column("registration_status", TypeName = "nvarchar(50)")] // ← đã sửa lại ở đây
        public RegistrationStatus? RegistrationStatus { get; set; }

        [Column("otp")]
        public string? Otp { get; set; }

        [Column("otp_expiry_date")]
        public DateTime? OtpExpiryDate { get; set; }

        [Column("zip_code")]
        public string? ZipCode { get; set; }

        [Column("expertise")]
        public string? Expertise { get; set; }

        [Column("years_of_experience")]
        public double? YearsOfExperience { get; set; }

        [Column("bio")]
        public string? Bio { get; set; }

        [Column("certificate")]
        public string? Certificate { get; set; }

        [Column("cv_url")]
        public string? CvUrl { get; set; }

        [Column("facebook_link")]
        public string? FacebookLink { get;  set; }

        [Column("points")]
        public long Points { get; set; } = 0;

        [Column("qr")]
        public string? Qr { get; set; }

        // Posts của User
        public ICollection<Post> Posts { get; set; } = new List<Post>();

        public ICollection<Course> Courses { get; set; } = new List<Course>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
        public ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<LessonProgress> LessonProgresses { get; set; } = new List<LessonProgress>();

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

    }
}
