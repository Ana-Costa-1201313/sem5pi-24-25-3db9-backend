using Backoffice.Domain.OperationTypes;
using Backoffice.Domain.Patients;
using Backoffice.Domain.Staffs;

namespace Backoffice.Domain.OperationRequests
{
    public static class OperationRequestMapper
    {
        public static OperationRequestDto ToDto(OperationRequest operationRequest)
        {
            if (operationRequest == null) throw new ArgumentNullException(nameof(operationRequest));

            return new OperationRequestDto
            {
                Id = operationRequest.Id.AsGuid(),
                OpTypeName = operationRequest.OpType.Name.ToString(),
                OpTypeId = operationRequest.OpTypeId.ToString(),
                DeadlineDate = operationRequest.DeadlineDate.ToString(),
                Priority = operationRequest.Priority,
                PatientName = operationRequest.Patient.FullName.ToString(),
                PatientId = operationRequest.PatientId.ToString(),
                DoctorName = operationRequest.Doctor.Name.ToString(),
                DoctorId = operationRequest.DoctorId.ToString(),
                Status = operationRequest.Status,
                Description = operationRequest.Description
            };
        }

        public static OperationRequest ToDomain(CreateOperationRequestDto dto, OperationType operationType, Patient patient, Staff doctor)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            
            return new OperationRequest(
                operationType,
                DateTime.Parse(dto.DeadlineDate),
                Enum.Parse<Priority>(dto.Priority),
                patient,
                doctor,
                dto.Description
            );
            //return null;
        }
    }
}