namespace Backoffice.Domain.Appointments
{
    public class AppointmentDto
    {
        public Guid AppointmentId { get; set; }
        public Guid OpRequestId { get; set; }
        public string StaffIds { get; set; }
        public Guid SurgeryRoomId { get; set; }
        public string DateTime { get; set; }
        public string Status { get; set; }
    }
}