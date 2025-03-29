using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Learning.Entity
{
    [Table("certificates")]
    public class Certificate : BaseEntity<long>
    {

        [Required]
        [Column("name")]
        public string Name { get; set; }


        [Column("issue_date")]
        public DateTime? IssueDate { get; set; }


        [ForeignKey("UserId")]
        public User User { get; set; }
        public long UserId { get; set; }


        [ForeignKey("CourseId")]
        public Course Course { get; set; }
        public long CourseId { get; set; }


        [Column("certificate_url")]
        public string? CertificateUrl { get; set; }


        [Column("description")]
        public string? Description { get; set; }
    }

}
