using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Backoffice.Domain.Specializations;

namespace Backoffice.Infraestructure.Specializations
{
    internal class SpecializationEntityTypeConfiguration : IEntityTypeConfiguration<Specialization>
    {
        public void Configure(EntityTypeBuilder<Specialization> builder)
        {
            builder.ToTable("Specialization", SchemaNames.Backoffice);
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id)
                .HasConversion(id => id.Value, value => new SpecializationId(value));
            builder.OwnsOne(b => b.Name, n =>
            {
                n.Property(c => c.Name).HasColumnName("Name").IsRequired();
                n.HasIndex(c => c.Name).IsUnique();
            });
            builder.Property<bool>(b => b.Active);
        }
    }
}