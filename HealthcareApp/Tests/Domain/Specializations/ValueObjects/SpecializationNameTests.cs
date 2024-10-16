using System;
using HealthcareApp.Domain.Specializations;
using HealthcareApp.Domain.Shared;
using Xunit;

namespace HealthcareApp.Tests;

public class SpecializationNameTests
{
    [Fact]
    public void TestSpecializationNameWithValidInputs()
    {
        var validName = "Cardiology";

        var specializationName = new SpecializationName(validName);

        Assert.Equal(validName, specializationName.Name);
    }

    [Fact]
    public void TestSpecializationNameWithNullName()
    {
        var exception = Assert.Throws<BusinessRuleValidationException>(() => new SpecializationName(null));

        Assert.Equal("Error: The operation name can't be null, empty or consist in only white spaces.", exception.Message);
    }

    [Fact]
    public void TestSpecializationNameWithEmptyName()
    {
        var exception = Assert.Throws<BusinessRuleValidationException>(() => new SpecializationName(""));

        Assert.Equal("Error: The operation name can't be null, empty or consist in only white spaces.", exception.Message);
    }

    [Fact]
    public void TestSpecializationNameWithWhiteSpacesName()
    {
        var exception = Assert.Throws<BusinessRuleValidationException>(() => new SpecializationName("   "));

        Assert.Equal("Error: The operation name can't be null, empty or consist in only white spaces.", exception.Message);
    }
}
