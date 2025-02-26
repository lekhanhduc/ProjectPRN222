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
        public DbSet<Course> Courses { get; set; }
        public DbSet<Role> Roles { get; set; }

    }
}
