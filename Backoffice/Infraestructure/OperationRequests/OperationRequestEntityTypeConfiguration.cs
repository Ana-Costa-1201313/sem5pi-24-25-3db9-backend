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

            builder.OwnsOne(b => b.OpType, opType => {
                opType.Property(p => p.Id).HasColumnName("OpTypeId");
                opType.Property(p => p.Name).HasColumnName("OpTypeName");
            });

            builder.Property(b => b.DeadlineDate).HasColumnName("DeadlineDate");
            builder.Property(b => b.Priority).HasColumnName("Priority");
        }
    }
}