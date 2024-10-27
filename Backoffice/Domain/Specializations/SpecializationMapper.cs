using System;
using Backoffice.Domain.Specializations;

namespace Backoffice.Domain.Specializations;

public class SpecializationMapper
{
    public SpecializationDto ToDto(Specialization specialization)
    {
        return new SpecializationDto
        {
            Id = specialization.Id.AsGuid(),
            Name = specialization.Name.Name
        };
    }

    public Specialization ToDomain(SpecializationDto dto)
    {
        return new Specialization(
            new SpecializationName(dto.Name)
        );
    }

    public Specialization ToDomain(CreatingSpecializationDto dto)
    {
        return new Specialization(
            new SpecializationName(dto.Name)
        );
    }

    //Only for tests
    public Specialization ToDomainForTests(string specName)
    {
        return new Specialization(
                new SpecializationName(specName));
    }

    //Only for tests
    public CreatingSpecializationDto ToCreateDtoForTests(string specName)
    {
        return new CreatingSpecializationDto(specName);
    }

}
