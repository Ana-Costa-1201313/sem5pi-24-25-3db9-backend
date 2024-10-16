using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HealthcareApp.Domain.Specializations;

namespace HealthcareApp.Infraestructure.Specializations
{
    internal class SpecializationEntityTypeConfiguration : IEntityTypeConfiguration<Specialization>
    {
        public void Configure(EntityTypeBuilder<Specialization> builder)
        {
            builder.ToTable("Specialization", SchemaNames.HealthcareApp);
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id)
                .HasConversion(id => id.Value, value => new SpecializationId(value));
            builder.OwnsOne(b => b.Name, n => 
            {
                n.Property( c => c.Name).HasColumnName("Name").IsRequired();
            });
            builder.Property<bool>(b => b.Active);
        }
    }
}