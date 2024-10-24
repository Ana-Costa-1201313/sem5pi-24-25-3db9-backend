using System;
using Backoffice.Domain.OperationRequests;
using Backoffice.Domain.OperationTypes;

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
                Priority = operationRequest.Priority.ToString(),
                PatientName = operationRequest.Patient.FullName.ToString(),
                PatientId = operationRequest.PatientId.ToString(),
                DoctorName = operationRequest.Doctor.Name.ToString(),
                DoctorId = operationRequest.DoctorId.ToString(),
                Status = operationRequest.Status.ToString(),
                Description = operationRequest.Description
            };
        }

        public static OperationRequest ToDomain(CreateOperationRequestDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            /*
            return new OperationRequest
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                OperationTypeId = dto.OperationTypeId,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt
            };*/
            return null;
        }
    }
}