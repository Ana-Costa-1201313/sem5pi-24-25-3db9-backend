using Microsoft.EntityFrameworkCore;
using Backoffice.Domain.Categories;
using Backoffice.Domain.Users;
using Backoffice.Infraestructure.Categories;
using Backoffice.Infraestructure.Users;

namespace Backoffice.Infraestructure
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
            //modelBuilder.HasSequence<int>("MechanographicNumbers");
            //modelBuilder.Entity<User>().Property(n => n.MechanographicNum).HasDefaultValueSql("NEXT VALUE FOR MechanographicNumbers");
        }
    }

}