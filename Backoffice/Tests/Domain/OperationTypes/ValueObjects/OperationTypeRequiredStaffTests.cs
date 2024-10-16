using System;
using Backoffice.Domain.OperationTypes;
using Backoffice.Domain.Shared;
using Backoffice.Domain.Specializations;
using Xunit;

namespace Backoffice.Tests;
public class OperationTypeRequiredStaffTests
{
    [Fact]
    public void TestOperationTypeRequiredStaffWithValidInputs()
    {
        var specializationName = new SpecializationName("Cardiology");
        var specialization = new Specialization(specializationName);

        var operationTypeRequiredStaff = new OperationTypeRequiredStaff(specialization, 3);

        Assert.Equal(specializationName.Name, operationTypeRequiredStaff.Specialization.Name.Name);
        Assert.Equal(3, operationTypeRequiredStaff.Total);
    }

    [Fact]
    public void TestOperationTypeRequiredStaffWithNullSpecialization()
    {

        var exception = Assert.Throws<BusinessRuleValidationException>(() => new OperationTypeRequiredStaff(null, 3));

        Assert.Equal("Error: The specialization can't be null.", exception.Message);
    }

    [Fact]
    public void TestOperationTypeRequiredStaffWithTotalEqualTo0()
    {
        var specializationName = new SpecializationName("Cardiology");
        var specialization = new Specialization(specializationName);
        var exception = Assert.Throws<BusinessRuleValidationException>(() => new OperationTypeRequiredStaff(specialization, 0));

        Assert.Equal("Error: The total number of required staff of a specialization can't be lower or equal to 0.", exception.Message);
    }
}
