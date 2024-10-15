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
        }
    }
}