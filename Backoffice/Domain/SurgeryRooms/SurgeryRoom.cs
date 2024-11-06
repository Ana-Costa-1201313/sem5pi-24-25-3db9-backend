using Backoffice.Domain.Shared;
using Backoffice.Domain.SurgeryRooms.ValueObjects;

namespace Backoffice.Domain.SurgeryRooms
{
    public class SurgeryRoom : Entity<SurgeryRoomId>, IAggregateRoot
    {
        //public SurgeryRoomId SurgeryRoomId { get; set; }
        public Int32 RoomNumber { get; set; }
        public SurgeryRoomType SurgeryRoomType { get; set; }
        public Int16 Capacity { get; set; }
        public SurgeryRoomStatus SurgeryRoomStatus { get; set; }
        public List<TimeSlot> MaintenanceSlots { get; set; }

        public SurgeryRoom(){}

        public SurgeryRoom(Int32 roomNumber, SurgeryRoomType surgeryRoomType, Int16 capacity, List<TimeSlot> maintenanceSlots)
        {
            this.Id = new SurgeryRoomId(Guid.NewGuid());

            if (roomNumber == default || roomNumber <= 0) throw new BusinessRuleValidationException("RoomNumber can't be 0 nor negative.");
            this.RoomNumber = roomNumber;

            if (surgeryRoomType == SurgeryRoomType.Null) throw new BusinessRuleValidationException("SurgeryRoomType can't be null.");
            this.SurgeryRoomType = surgeryRoomType;

            if (capacity == default || capacity <= 0) throw new BusinessRuleValidationException("Capacity can't be 0 nor negative.");
            this.Capacity = capacity;

            //if (maintenanceSlots == null || !maintenanceSlots.Any()) throw new BusinessRuleValidationException("MaintenanceSlots can't be null or empty.");
            this.SurgeryRoomStatus = SurgeryRoomStatus.Available;
            this.MaintenanceSlots = maintenanceSlots;
        }
    }
}