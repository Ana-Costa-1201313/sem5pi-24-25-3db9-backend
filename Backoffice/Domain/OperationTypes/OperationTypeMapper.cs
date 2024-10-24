using System;
using Backoffice.Domain.Specializations;

namespace Backoffice.Domain.OperationTypes;

public static class OperationTypeMapper
{
    public static OperationTypeDto ToDto(OperationType operationType)
    {
        return new OperationTypeDto
        {
            Id = operationType.Id.AsGuid(),
            Name = operationType.Name.Name,
            AnesthesiaPatientPreparationInMinutes = (int)operationType.Duration.AnesthesiaPatientPreparationInMinutes.TotalMinutes,
            SurgeryInMinutes = (int)operationType.Duration.SurgeryInMinutes.TotalMinutes,
            CleaningInMinutes = (int)operationType.Duration.CleaningInMinutes.TotalMinutes,
            RequiredStaff = operationType.RequiredStaff.Select(staff => new RequiredStaffDto
            {
                Specialization = staff.Specialization.Name.Name,
                Total = staff.Total
            }).ToList(),
            Active = operationType.Active
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

    //Only for tests
    public static OperationType ToDomainForTests(string OpTypeName,
                                            int OpTypeDurationPhase1,
                                            int OpTypeDurationPhase2,
                                            int OpTypeDurationPhase3,
                                            List<(string SpecializationName, int Total)> requiredStaff)
    {

        var staffList = requiredStaff
        .Select(staff => new OperationTypeRequiredStaff(
            new Specialization(new SpecializationName(staff.SpecializationName)), staff.Total))
        .ToList();


        return new OperationType(
                new OperationTypeName(OpTypeName),
                new OperationTypeDuration(OpTypeDurationPhase1, OpTypeDurationPhase2, OpTypeDurationPhase3),
                staffList);
    }

    //Only for tests
    public static CreatingOperationTypeDto ToCreateDtoForTests(string opTypeName,
                                                    int opTypeDurationPhase1,
                                                    int opTypeDurationPhase2,
                                                    int opTypeDurationPhase3,
                                                    List<(string SpecializationName, int Total)> requiredStaff)
    {
        var staffList = requiredStaff
            .Select(staff => new RequiredStaffDto
            {
                Specialization = staff.SpecializationName,
                Total = staff.Total
            })
            .ToList();

        return new CreatingOperationTypeDto(
            opTypeName,
            opTypeDurationPhase1,
            opTypeDurationPhase2,
            opTypeDurationPhase3,
            staffList);
    }
}
