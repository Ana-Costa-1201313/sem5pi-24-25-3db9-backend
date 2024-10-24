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
                .IsRequired()
                .HasConversion(id => id.Value, value => new OperationRequestId(value));

            builder.Property(or => or.DeadlineDate)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(or => or.Priority)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(or => or.Status)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(or => or.Description)
                .HasMaxLength(500);

            builder.HasOne(or => or.OpType)
                .WithMany()
                .HasForeignKey(or => or.OpTypeId)
                .IsRequired();

            builder.HasOne(or => or.Patient)
                .WithMany()
                .HasForeignKey(or => or.PatientId)
                .IsRequired();

            builder.HasOne(or => or.Doctor)
                .WithMany()
                .HasForeignKey(or => or.DoctorId)
                .IsRequired();
        }
    }
}