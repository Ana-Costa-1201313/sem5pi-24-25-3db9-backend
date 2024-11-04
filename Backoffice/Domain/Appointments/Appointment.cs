using System;
using Azure;
using Newtonsoft.Json;
using Backoffice.Domain.Shared;
using Backoffice.Domain.Appointments.ValueObjects;
using Backoffice.Domain.OperationRequests;
using Backoffice.Domain.Staffs;
using Backoffice.Domain.SurgeryRooms;

namespace Backoffice.Domain.Appointments
{
    public class Appointment : Entity<AppointmentId>, IAggregateRoot
    {
        //public AppointmentId Id { get; set; }
        public OperationRequestId OperationRequestId { get; set; }
        public OperationRequest OperationRequest { get; set; }
        public List<StaffId> StaffIds { get; set; }
        public List<Staff> Staff { get; set; }
        public SurgeryRoomId SurgeryRoomId { get; set; }
        public SurgeryRoom SurgeryRoom { get; set; }
        public DateTime DateTime { get; set; }
        public AppointmentStatus Status { get; set; }

        private Appointment() { }

        public Appointment(OperationRequest operationRequest, List<Staff> staff, SurgeryRoom surgeryRoom, DateTime dateTime)
        {
            this.Id = new AppointmentId(Guid.NewGuid());

            if (operationRequest == null) throw new BusinessRuleValidationException("OperationRequest can't be null.");
            this.OperationRequest = operationRequest;
            this.OperationRequestId = operationRequest.Id;

            if (staff == null || !staff.Any()) throw new BusinessRuleValidationException("Staff can't be null or empty.");
            this.Staff = staff;
            this.StaffIds = staff.Select(x => x.Id).ToList();

            if (surgeryRoom == null) throw new BusinessRuleValidationException("SurgeryRoom can't be null.");
            this.SurgeryRoom = surgeryRoom;
            this.SurgeryRoomId = surgeryRoom.Id;

            if (dateTime == default) throw new BusinessRuleValidationException("DateTime can't be default value.");
            this.DateTime = dateTime;
            
            this.Status = AppointmentStatus.Scheduled;
        }
    }
}