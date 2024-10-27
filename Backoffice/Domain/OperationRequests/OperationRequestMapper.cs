using Backoffice.Domain.OperationTypes;
using Backoffice.Domain.Patients;
using Backoffice.Domain.Staffs;

namespace Backoffice.Domain.OperationRequests
{
    public static class OperationRequestMapper
    {
public static OperationRequestDto ToDto(OperationRequest operationRequest, OperationTypeName opTypeName, string patientName, string doctorName)
{
    if (operationRequest == null) throw new ArgumentNullException(nameof(operationRequest));

    return new OperationRequestDto
    {
        Id = operationRequest.Id?.AsGuid() ?? Guid.Empty,
        OpTypeName = opTypeName,
        OpTypeId = operationRequest.OpTypeId?.AsGuid() ?? Guid.Empty,
        DeadlineDate = operationRequest.DeadlineDate.ToString("yyyy-MM-ddTHH:mm:ss"),
        Priority = operationRequest.Priority.ToString(),
        PatientName = patientName,
        PatientId = operationRequest.PatientId?.AsGuid() ?? Guid.Empty,
        DoctorName = doctorName,
        DoctorId = operationRequest.DoctorId?.AsGuid() ?? Guid.Empty,
        Status = operationRequest.Status.ToString(),
        Description = operationRequest.Description ?? string.Empty
    };
}

        public static OperationRequest ToDomain(CreateOperationRequestDto dto, OperationType operationType, Patient patient, Staff doctor)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            
            return new OperationRequest(
                operationType,
                DateTime.Parse(dto.DeadlineDate),
                Enum.TryParse<Priority>(dto.Priority, out var priority) ? priority : (Priority?)null,
                patient,
                doctor,
                dto.Description
            );
            //return null;
        }

        public static OperationRequest ToDomainTests(OperationType operationType, DateTime deadlineDate, Priority priority, Patient patient, Staff doctor, string description)
        {
            return new OperationRequest(
                operationType,
                deadlineDate,
                priority,
                patient,
                doctor,
                description
            );
        }/*
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            
            return new OperationRequest(
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