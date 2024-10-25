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

            builder.HasKey(or => or.Id);
            builder.Property(or => or.Id)
                .HasConversion(id => id.Value, value => new OperationRequestId(value));

            builder.Property(or => or.DeadlineDate)
                .HasMaxLength(50);

            builder.Property(or => or.Priority)
                .HasMaxLength(50);

            builder.Property(or => or.Status)
                .HasMaxLength(50);

            builder.Property(or => or.Description)
                .HasMaxLength(500);

            builder.HasOne(or => or.OpType)
                .WithOne()
                .HasForeignKey<OperationRequest>(or => or.OpTypeId);

            builder.HasOne(or => or.Patient)
                .WithOne()
                .HasForeignKey<OperationRequest>(or => or.PatientId);

            builder.HasOne(or => or.Doctor)
                .WithOne()
                .HasForeignKey<OperationRequest>(or => or.DoctorId);
        }
    }
}