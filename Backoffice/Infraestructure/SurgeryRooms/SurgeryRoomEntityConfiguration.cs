using Backoffice.Domain.SurgeryRooms;
using Backoffice.Domain.SurgeryRooms.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backoffice.Infraestructure.SurgeryRooms
{
    internal class SurgeryRoomEntityTypeConfiguration : IEntityTypeConfiguration<SurgeryRoom>
    {
        public void Configure(EntityTypeBuilder<SurgeryRoom> builder)
        {
            builder.ToTable("SurgeryRooms", SchemaNames.Backoffice);

            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id)
                .HasConversion(id => id.Value, value => new SurgeryRoomId(value));

            builder.Property(b => b.RoomNumber)
                .HasColumnName("RoomNumber").IsRequired();

            builder.Property(b => b.SurgeryRoomType)
                .HasConversion(
                    s => s.ToString(),
                    s => (SurgeryRoomType)Enum.Parse(typeof(SurgeryRoomType), s))
                .HasColumnName("SurgeryRoomType")
                .IsRequired();

            builder.Property(b => b.Capacity)
                .HasColumnName("Capacity").IsRequired();

            builder.Property(b => b.SurgeryRoomStatus)
                .HasConversion(
                    s => s.ToString(),
                    s => (SurgeryRoomStatus)Enum.Parse(typeof(SurgeryRoomStatus), s))
                .HasColumnName("SurgeryRoomStatus")
                .IsRequired();

            builder.OwnsMany(b => b.MaintenanceSlots, a =>
            {
                a.ToTable("MaintenanceSlot");
                a.HasKey("SurgeryRoomId", "StartTime", "EndTime");
                a.WithOwner().HasForeignKey("SurgeryRoomId");
            });
        }
    }
}