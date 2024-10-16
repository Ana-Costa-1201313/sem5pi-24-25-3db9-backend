using System;
using HealthcareApp.Domain.Specializations;

namespace HealthcareApp.Domain.OperationTypes;

public static class OperationTypeMapper
{
    public static OperationTypeDto ToDto(OperationType operationType)
    {
        return new OperationTypeDto
        {
            Name = operationType.Name.Name,
            AnesthesiaPatientPreparationInMinutes = (int)operationType.Duration.AnesthesiaPatientPreparationInMinutes.TotalMinutes,
            SurgeryInMinutes = (int)operationType.Duration.SurgeryInMinutes.TotalMinutes,
            CleaningInMinutes = (int)operationType.Duration.CleaningInMinutes.TotalMinutes,
            RequiredStaff = operationType.RequiredStaff.Select(staff => new RequiredStaffDto
            {
                Specialization = staff.Specialization.Name.Name,
                Total = staff.Total
            }).ToList()
        };
    }

    public static OperationType ToDomain(OperationTypeDto dto)
    {
        return new OperationType(
            new OperationTypeName(dto.Name),
            new OperationTypeDuration(dto.AnesthesiaPatientPreparationInMinutes, dto.SurgeryInMinutes, dto.CleaningInMinutes),
            dto.RequiredStaff.Select(staffDto => new OperationTypeRequiredStaff(
                new Specialization(new SpecializationName(staffDto.Specialization)), staffDto.Total)).ToList()
        );
    }

    public static OperationType ToDomain(CreatingOperationTypeDto dto)
    {
        return new OperationType(
            new OperationTypeName(dto.Name),
            new OperationTypeDuration(dto.AnesthesiaPatientPreparationInMinutes, dto.SurgeryInMinutes, dto.CleaningInMinutes),
            dto.RequiredStaff.Select(staffDto => new OperationTypeRequiredStaff(
                new Specialization(new SpecializationName(staffDto.Specialization)), staffDto.Total)).ToList()
        );
    }
}
