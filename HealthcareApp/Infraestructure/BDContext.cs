using Microsoft.EntityFrameworkCore;
using HealthcareApp.Domain.Categories;
using HealthcareApp.Infraestructure.Categories;
using HealthcareApp.Domain.OperationTypes;
using HealthcareApp.Infraestructure.OperationTypes;
using HealthcareApp.Domain.Specializations;
using HealthcareApp.Infraestructure.Specializations;


namespace HealthcareApp.Infraestructure
{
    public class BDContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<OperationType> OperationTypes { get; set; }
        public DbSet<Specialization> Specializations { get; set; }

        public BDContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OperationTypeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SpecializationEntityTypeConfiguration());
        }
    }

}