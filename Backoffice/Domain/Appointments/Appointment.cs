using System;
using Backoffice.Domain.OperationRequests;

namespace Backoffice.Domain.Appointments
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public OperationRequestId OperationRequestId { get; set; }
        public Guid RoomId { get; set; }
        public DateTime DateTime { get; set; }
        public AppointmentStatus Status { get; set; }
    }
}