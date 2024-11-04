using Backoffice.Domain.SurgeryRooms.ValueObjects;

namespace Backoffice.Domain.SurgeryRooms
{
    public class SurgeryRoom
    {
        public SurgeryRoomId SurgeryRoomId { get; set; }
        public Int32 RoomNumber { get; set; }
        public SurgeryRoomType SurgeryRoomType { get; set; }
        public Int16 Capacity { get; set; }
        public SurgeryRoomStatus SurgeryRoomStatus { get; set; }
        public List<DateTime> MaintenanceSlots { get; set; }

        public SurgeryRoom()
        {
            MaintenanceSlots = new List<DateTime>();
        }
    }
}