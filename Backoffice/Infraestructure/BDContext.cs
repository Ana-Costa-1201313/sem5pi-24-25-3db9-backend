using Microsoft.EntityFrameworkCore;
using Backoffice.Domain.Categories;
using Backoffice.Domain.Users;
using Backoffice.Domain.Staffs;
using Backoffice.Domain.Patients;
using Backoffice.Domain.OperationRequests;
using Backoffice.Infraestructure.Categories;
using Backoffice.Infraestructure.Users;
using Backoffice.Infraestructure.Staffs;
using Backoffice.Infraestructure.Patients;
using Backoffice.Infraestructure.OperationRequests;
using Backoffice.Domain.OperationTypes;
using Backoffice.Infraestructure.OperationTypes;
using Backoffice.Domain.Specializations;
using Backoffice.Infraestructure.Specializations;
using Backoffice.Domain.Logs;
using Backoffice.Infraestructure.Logs;

namespace Backoffice.Infraestructure
{
    public class BDContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<OperationRequest> OperationRequests { get; set; }
        public DbSet<OperationType> OperationTypes { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Log> Logs { get; set; }

        public BDContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new StaffEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PatientEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OperationRequestEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OperationTypeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SpecializationEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LogEntityTypeConfiguration());

            modelBuilder.Ignore<RequiredStaffDto>();
        }
    }

}