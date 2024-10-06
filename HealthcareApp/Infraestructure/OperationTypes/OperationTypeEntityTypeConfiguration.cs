using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HealthcareApp.Domain.OperationTypes;

namespace HealthcareApp.Infraestructure.OperationTypes
{
    internal class OperationTypeEntityTypeConfiguration : IEntityTypeConfiguration<OperationType>
    {
        public void Configure(EntityTypeBuilder<OperationType> builder)
        {
            builder.ToTable("OperationTypes", SchemaNames.HealthcareApp);
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Description);
            builder.Property(b => b.Name);
            builder.Property(b => b.Duration);
            builder.Property(b => b.RequiredStaff);
            builder.Property<bool>(b => b.Active);
        }
    }
}