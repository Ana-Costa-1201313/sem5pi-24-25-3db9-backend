using System;
using Backoffice.Domain.Appointments.ValueObjects;
using Backoffice.Domain.OperationRequests;
using Backoffice.Domain.Staffs;
using Backoffice.Domain.SurgeryRooms;

namespace Backoffice.Domain.Appointments
{
    public class Appointment
    {
        public AppointmentId Id { get; set; }
        public OperationRequestId OperationRequestId { get; set; }
        public List<StaffId> StaffIds { get; set; }
        public SurgeryRoomId SurgeryRoomId { get; set; }
        public DateTime DateTime { get; set; }
        public AppointmentStatus Status { get; set; }
    }
}