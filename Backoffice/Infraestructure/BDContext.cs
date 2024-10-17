using Microsoft.EntityFrameworkCore;
using Backoffice.Domain.Categories;
using Backoffice.Infraestructure.Categories;
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
        public DbSet<OperationType> OperationTypes { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Log> Logs { get; set; }

        public BDContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OperationTypeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SpecializationEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LogEntityTypeConfiguration());
        }
    }

}