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

            builder.OwnsOne(b => b.Name, n =>
            {
                n.Property(c => c.Name).HasColumnName("Name").IsRequired();
            });

            builder.OwnsOne(b => b.Duration);

            builder.OwnsMany(b => b.RequiredStaff, rs =>
            {
                rs.ToTable("RequiredStaff");

                rs.HasKey("SpecializationId", "OperationTypeId");

                rs.Property(rs => rs.SpecializationId)
                    .HasColumnName("SpecializationId")
                    .IsRequired();

                rs.Property(staff => staff.Total)
                    .HasColumnName("Total")
                    .IsRequired();

                rs.WithOwner().HasForeignKey("OperationTypeId");
            });

            builder.Property<bool>(b => b.Active);
        }
    }
}