using Microsoft.EntityFrameworkCore;
using Backoffice.Domain.Categories;
using Backoffice.Domain.Users;
using Backoffice.Domain.Staff;
using Backoffice.Domain.Patients;
using Backoffice.Infraestructure.Categories;
using Backoffice.Infraestructure.Users;
using Backoffice.Infraestructure.Staffs;
using Backoffice.Infraestructure.Patients;

namespace Backoffice.Infraestructure
{
    public class BDContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Patient> Patients { get; set; }

        public BDContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new StaffEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PatientEntityTypeConfiguration());
        }
    }

}