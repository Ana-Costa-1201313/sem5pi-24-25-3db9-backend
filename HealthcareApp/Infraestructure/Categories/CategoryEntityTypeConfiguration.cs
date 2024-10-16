using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HealthcareApp.Domain.Categories;

namespace HealthcareApp.Infraestructure.Categories
{
    internal class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories", SchemaNames.HealthcareApp);
            builder.HasKey(b => b.Id);

            // Assuming CategoryId is a wrapper around Guid
            builder.Property(b => b.Id)
                .HasConversion(id => id.Value, value => new CategoryId(value));
            builder.Property<bool>("_active").HasColumnName("Active");
        }
    }
}