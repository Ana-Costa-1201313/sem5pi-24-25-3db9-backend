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
            builder.OwnsOne(b => b.Name, n => 
            {
                n.Property( c => c.Name).HasColumnName("Name").IsRequired();
            });
            builder.Property(b => b.Duration);
            builder.OwnsMany(b => b.RequiredStaff, rs =>
            {
                rs.ToTable("RequiredStaff");

                rs.Property(staff => staff.Specialization)
                .HasColumnName("Specialization")
                .IsRequired();

                rs.Property(staff => staff.Total)
                .HasColumnName("Total")
                .IsRequired();

                rs.WithOwner()
                .HasForeignKey("OperationTypeId");

                rs.HasKey("OperationTypeId", "Specialization");
            });
            builder.Property<bool>(b => b.Active);
        }
    }
}