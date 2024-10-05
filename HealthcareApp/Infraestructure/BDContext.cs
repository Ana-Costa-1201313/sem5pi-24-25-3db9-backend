using Microsoft.EntityFrameworkCore;
using HealthcareApp.Domain.Categories;
using HealthcareApp.Domain.Users;
using HealthcareApp.Infraestructure.Categories;
using HealthcareApp.Infraestructure.Users;

namespace HealthcareApp.Infraestructure
{
    public class BDContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }

        public BDContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
        }
    }

}