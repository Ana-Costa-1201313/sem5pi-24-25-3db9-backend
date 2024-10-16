using System;
using HealthcareApp.Domain.Specializations;

namespace HealthcareApp.Domain.Specializations;

public static class SpecializationMapper
{
    public static SpecializationDto ToDto(Specialization specialization)
    {
        return new SpecializationDto
        {
            Id = specialization.Id.AsGuid(),
            Name = specialization.Name.Name
        };
    }

    public static Specialization ToDomain(SpecializationDto dto)
    {
        return new Specialization(
            new SpecializationName(dto.Name)
        );
    }

    public static Specialization ToDomain(CreatingSpecializationDto dto)
    {
        return new Specialization(
            new SpecializationName(dto.Name)
        );
    }

}
