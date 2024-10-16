using Microsoft.EntityFrameworkCore;
using Backoffice.Domain.Categories;
using Backoffice.Domain.Users;
using Backoffice.Domain.Staffs;
using Backoffice.Infraestructure.Categories;
using Backoffice.Infraestructure.Users;
using Backoffice.Infraestructure.Staffs;

namespace Backoffice.Infraestructure
{
    public class BDContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Staff> Staff { get; set; }

        public BDContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new StaffEntityTypeConfiguration());
        }
    }

}