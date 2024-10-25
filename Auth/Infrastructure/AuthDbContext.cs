using Microsoft.EntityFrameworkCore;
using Auth.Domain.Users;
using Auth.Infrastructure.Users;


namespace Auth.Infrastructure
{
    public class AuthDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
    public AuthDbContext(DbContextOptions options) : base(options)
        {
        }

            
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
        }
    }
}