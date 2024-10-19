using System;
using Backoffice.Domain.Specializations;

namespace Backoffice.Domain.Specializations;

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

    //Only for tests
    public static Specialization ToDomainForTests(string specName)
    {
        return new Specialization(
                new SpecializationName(specName));
    }

    //Only for tests
    public static CreatingSpecializationDto ToCreateDtoForTests(string specName)
    {
        return new CreatingSpecializationDto(specName);
    }

}
