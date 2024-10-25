using Microsoft.EntityFrameworkCore;
using Backoffice.Domain.Users;
using Backoffice.Domain.Staffs;
using Backoffice.Domain.OperationTypes;
using Backoffice.Infraestructure.OperationTypes;
using Backoffice.Domain.Specializations;
using Backoffice.Infraestructure.Specializations;
using Backoffice.Domain.Logs;
using Backoffice.Infraestructure.Logs;
using Backoffice.Infraestructure.Users;
using Backoffice.Infraestructure.Staffs;


namespace Backoffice.Infraestructure
{
    public class BDContext : DbContext
    {
        public DbSet<OperationType> OperationTypes { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Staff> Staff { get; set; }

        public BDContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new StaffEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OperationTypeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SpecializationEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LogEntityTypeConfiguration());

            modelBuilder.Ignore<RequiredStaffDto>();
        }
    }
}