using E_Learning.Entity;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Data
{
    public class ELearningDbContext : DbContext
    {
        public ELearningDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<LessonProgress> LessonProgresses { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Advertisement> Advertisements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Certificate>()
                .HasOne(c => c.User)
                .WithMany(u => u.Certificates)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Certificate>()
                .HasOne(c => c.Course)
                .WithMany(c => c.Certificates)
                .HasForeignKey(c => c.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.User)
                .WithMany(u => u.Enrollments)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.User)
                .WithMany(u => u.Favorites)
                 .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.Course)
                .WithMany(c => c.Favorites)
                .HasForeignKey(f => f.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Quan hệ 1-N giữa Comment và Post
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            // Quan hệ Cha - Con trong Comment
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.ParentComment)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LessonProgress>()
                .HasOne(lp => lp.User)
                .WithMany(u => u.LessonProgresses)
                .HasForeignKey(lp => lp.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LessonProgress>()
                .HasOne(lp => lp.Lesson)
                .WithMany(l => l.LessonProgresses)
                .HasForeignKey(lp => lp.LessonId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }


    }
}
