using Microsoft.EntityFrameworkCore;
using HealthcareApp.Domain.Categories;
using HealthcareApp.Infraestructure.Categories;
using HealthcareApp.Domain.OperationTypes;
using HealthcareApp.Infraestructure.OperationTypes;


namespace HealthcareApp.Infraestructure
{
    public class BDContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<OperationType> OperationTypes { get; set; }

        public BDContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OperationTypeEntityTypeConfiguration());
        }
    }

}