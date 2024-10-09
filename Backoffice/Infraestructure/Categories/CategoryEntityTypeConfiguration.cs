using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Backoffice.Domain.Categories;

namespace Backoffice.Infraestructure.Categories
{
    internal class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories", SchemaNames.Backoffice);
            builder.HasKey(b => b.Id);
            builder.Property<bool>("_active").HasColumnName("Active");
        }
    }
}