using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Backoffice.Domain.OperationRequests;

namespace Backoffice.Infraestructure.OperationRequests
{
    internal class OperationRequestEntityTypeConfiguration : IEntityTypeConfiguration<OperationRequest>
    {
        public void Configure(EntityTypeBuilder<OperationRequest> builder)
        {
            builder.ToTable("OperationRequests", SchemaNames.Backoffice);

            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id)
                .HasConversion(id => id.Value, value => new OperationRequestId(value));

            builder.Property(b => b.DeadlineDate)
                .HasColumnName("DeadlineDate").IsRequired();

            builder.Property(b => b.Priority)
                .HasColumnName("Priority").IsRequired();

            builder.Property(b => b.Status)
                .HasColumnName("Status").IsRequired();

            builder.Property(b => b.Description)
                .HasColumnName("Description").IsRequired();

            builder.HasOne(b => b.OpType)
                .WithOne()
                .HasForeignKey<OperationRequest>(b => b.OpTypeId);

            builder.HasOne(b => b.Patient)
                .WithOne()
                .HasForeignKey<OperationRequest>(b => b.PatientId);

            builder.HasOne(b => b.Doctor)
                .WithOne()
                .HasForeignKey<OperationRequest>(b => b.DoctorId);
        }
    }
}