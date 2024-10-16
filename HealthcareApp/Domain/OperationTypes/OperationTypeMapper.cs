using System;

namespace HealthcareApp.Domain.OperationTypes;

public static class OperationTypeMapper
{
/*    public static OperationTypeDto ToDto(OperationType operationType)
    {
        return new OperationTypeDto
        {
            Name = operationType.Name.Value, // Assuming OperationTypeName is a value object
            AnesthesiaPatientPreparationInMinutes = operationType.Duration.AnesthesiaPatientPreparationInMinutes,
            SurgeryInMinutes = operationType.Duration.SurgeryInMinutes,
            CleaningInMinutes = operationType.Duration.CleaningInMinutes,
            RequiredStaff = operationType.RequiredStaff.Select(staff => new OperationTypeRequiredStaffDto
            {
                Specialization = staff.Specialization.Name.Value, // Assuming SpecializationName is a value object
                Total = staff.Total
            }).ToList()
        };
    }

    // Mapping DTO back to domain model
    public static OperationType ToDomain(OperationTypeDto dto)
    {
        return new OperationType(
            new OperationTypeName(dto.Name),
            new OperationTypeDuration(dto.AnesthesiaPatientPreparationInMinutes, dto.SurgeryInMinutes, dto.CleaningInMinutes),
            dto.RequiredStaff.Select(staffDto => new OperationTypeRequiredStaff(
                new Specialization(new SpecializationName(staffDto.Specialization)), staffDto.Total)).ToList()
        );
    }*/
}
