namespace Backoffice.Domain.Appointments
{
    public class CreateAppointmentDto
    {
        public string OpRequestId { get; set; }
        public string StaffIds { get; set; }
        public string SurgeryRoomId { get; set; }
        public string DateTime { get; set; }
    }
}