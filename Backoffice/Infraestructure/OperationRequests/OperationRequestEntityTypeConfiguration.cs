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
                .WithMany()
                .HasForeignKey(b => b.OpTypeId)
                .OnDelete(DeleteBehavior.Restrict);;

            builder.HasOne(b => b.Patient)
                .WithMany()
                .HasForeignKey(b => b.PatientId)
                .OnDelete(DeleteBehavior.Restrict);;

            builder.HasOne(b => b.Doctor)
                .WithMany()
                .HasForeignKey(b => b.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}