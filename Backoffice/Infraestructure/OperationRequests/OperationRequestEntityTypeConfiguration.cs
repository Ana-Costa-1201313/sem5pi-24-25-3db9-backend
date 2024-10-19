using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Backoffice.Domain.OperationRequests;

namespace Backoffice.Infraestructure.OperationRequests
{
    internal class OperationRequestEntityTypeConfiguration : IEntityTypeConfiguration<OperationRequest>
    {
        public void Configure(EntityTypeBuilder<OperationRequest> builder)
        {
            builder.ToTable("OperationRequest", SchemaNames.Backoffice);
            builder.HasKey(b => b.Id); 
            builder.Property(b => b.Id)
                .HasConversion(id => id.Value, value => new OperationRequestId(value));
        }
    }
}