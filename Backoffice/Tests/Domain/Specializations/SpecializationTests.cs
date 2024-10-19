using System;
using Backoffice.Domain.Specializations;
using Backoffice.Domain.Shared;
using Moq;
using Xunit;

namespace Backoffice.Tests;

public class SpecializationTests
{

    [Fact]
    public void TestSpecializationWithValidSpecializationName()
    {
        var specializationName = new SpecializationName("Cardiology");

        var specialization = new Specialization(specializationName);

        Assert.Equal(specializationName, specialization.Name);
        Assert.True(specialization.Active);
        Assert.NotNull(specialization.Id);
    }

    [Fact]
    public void TestChangeSpecializationNameWhenSpecializationActive()
    {
        var specializationName = new SpecializationName("Cardiology");
        var specialization = new Specialization(specializationName);

        var newSpecializationName = new SpecializationName("Neurology");

        specialization.ChangeName(newSpecializationName);

        Assert.Equal(newSpecializationName, specialization.Name);
    }

    [Fact]
    public void TestChangeSpecializationNameWhenSpecializationInative()
    {
        var specializationName = new SpecializationName("Cardiology");
        var specialization = new Specialization(specializationName);

        var newSpecializationName = new SpecializationName("Neurology");

        specialization.MarkAsInative();

        var exception = Assert.Throws<BusinessRuleValidationException>(() => specialization.ChangeName(newSpecializationName));
        Assert.Equal("It is not possible to change the name to an inactive specialization.", exception.Message);
    }

    [Fact]
    public void TestMarkSpecializationAsInative()
    {
        var specializationName = new SpecializationName("Cardiology");
        var specialization = new Specialization(specializationName);

        specialization.MarkAsInative();

        Assert.False(specialization.Active);
    }

    [Fact]
    public void TestMarkInativeSpecializationAsInative()
    {
        var specializationName = new SpecializationName("Cardiology");
        var specialization = new Specialization(specializationName);

        specialization.MarkAsInative();

        var exception = Assert.Throws<BusinessRuleValidationException>(() => specialization.MarkAsInative());
        Assert.Equal("Error: This specialization is already inactive.", exception.Message);
    }
}
