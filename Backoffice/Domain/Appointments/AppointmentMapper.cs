using Backoffice.Domain.OperationRequests;
using Backoffice.Domain.OperationTypes;
using Backoffice.Domain.Staffs;
using Backoffice.Domain.SurgeryRooms;

namespace Backoffice.Domain.Appointments
{
    public static class AppointmentMapper
    {
        public static AppointmentDto ToDto(Appointment appointment)
        {
            if (appointment == null) throw new ArgumentNullException(nameof(appointment));

            return new AppointmentDto
            {
                AppointmentId = appointment.Id?.AsGuid() ?? Guid.Empty,
                OpRequestId = appointment.OperationRequestId?.AsGuid() ?? Guid.Empty,
                StaffIds = appointment.StaffIds.ToString(),
                SurgeryRoomId = appointment.SurgeryRoomId?.AsGuid() ?? Guid.Empty,
                DateTime = appointment.DateTime.ToString(),
                Status = appointment.Status.ToString()
            };
        }

        public static Appointment ToDomain(CreateAppointmentDto dto, OperationRequest operationRequest, List<Staff> staff, SurgeryRoom surgeryRoom)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new Appointment(
                operationRequest,
                staff,
                surgeryRoom,
                DateTime.Parse(dto.DateTime)
            );
            //return null;
        }/*

        public static Appointment ToDomainTests(OperationType operationType, DateTime deadlineDate, Priority priority, Patient patient, Staff doctor, string description)
        {
            return new Appointment(
                operationType,
                deadlineDate,
                priority,
                patient,
                doctor,
                description
            );
        }
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            
            return new Appointment(
                operationType,
                DateTime.Parse(dto.DeadlineDate),
                Enum.TryParse<Priority>(dto.Priority, out var priority) ? priority : (Priority?)null,
                patient,
                doctor,
                dto.Description
            );
            //return null;
        }*/
    }
}