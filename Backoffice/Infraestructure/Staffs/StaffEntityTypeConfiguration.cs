using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Backoffice.Domain.Staff;

namespace Backoffice.Infraestructure.Staffs
{
    internal class StaffEntityTypeConfiguration : IEntityTypeConfiguration<Staff>
    {
        public void Configure(EntityTypeBuilder<Staff> builder)
        {
            builder.ToTable("Staff", SchemaNames.Backoffice);
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Id)
                .HasConversion(id => id.Value, value => new StaffId(value));
        }
    }
}