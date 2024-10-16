using System;
using Backoffice.Domain.OperationTypes;
using Backoffice.Domain.Shared;
using Xunit;

namespace Backoffice.Tests;
public class OperationTypeDurationTests
{
    [Fact]
    public void TestOperationTypeDurationWithValidInputs()
    {
        int anesthesiaPatientPreparationInMinutes = 30;
        int surgeryInMinutes = 120;
        int cleaningInMinutes = 60;

        var operationTypeDuration = new OperationTypeDuration(
            anesthesiaPatientPreparationInMinutes,
            surgeryInMinutes,
            cleaningInMinutes
        );

        Assert.Equal(TimeSpan.FromMinutes(anesthesiaPatientPreparationInMinutes), operationTypeDuration.AnesthesiaPatientPreparationInMinutes);
        Assert.Equal(TimeSpan.FromMinutes(surgeryInMinutes), operationTypeDuration.SurgeryInMinutes);
        Assert.Equal(TimeSpan.FromMinutes(cleaningInMinutes), operationTypeDuration.CleaningInMinutes);
    }

    [Fact]
    public void TestOperationTypeDurationWith0MinutesAnesthesia()
    {
        int anesthesiaPatientPreparationInMinutes = 0;
        int surgeryInMinutes = 120;
        int cleaningInMinutes = 60;

        var exception = Assert.Throws<BusinessRuleValidationException>(() =>
            new OperationTypeDuration(anesthesiaPatientPreparationInMinutes, surgeryInMinutes, cleaningInMinutes));

        Assert.Equal("Error: The anesthesia/preparation duration must be more than 0 minutes.", exception.Message);
    }


    [Fact]
    public void TestOperationTypeDurationWith0MinutesSurgery()
    {
        int anesthesiaPatientPreparationInMinutes = 30;
        int surgeryInMinutes = 0;
        int cleaningInMinutes = 60;

        var exception = Assert.Throws<BusinessRuleValidationException>(() =>
            new OperationTypeDuration(anesthesiaPatientPreparationInMinutes, surgeryInMinutes, cleaningInMinutes));

        Assert.Equal("Error: The surgery duration must be more than 0 minutes.", exception.Message);
    }


    [Fact]
    public void TestOperationTypeDurationWith0MinutesCleaning()
    {
        int anesthesiaPatientPreparationInMinutes = 30;
        int surgeryInMinutes = 120;
        int cleaningInMinutes = 0;

        var exception = Assert.Throws<BusinessRuleValidationException>(() =>
            new OperationTypeDuration(anesthesiaPatientPreparationInMinutes, surgeryInMinutes, cleaningInMinutes));

        Assert.Equal("Error: The cleaning duration must be more than 0 minutes.", exception.Message);
    }



    [Fact]
    public void TestOperationTypeDurationWithNegativeInputs()
    {
        int anesthesiaPatientPreparationInMinutes = -10;
        int surgeryInMinutes = -30;
        int cleaningInMinutes = -5;

        var anesthesiaException = Assert.Throws<BusinessRuleValidationException>(() =>
            new OperationTypeDuration(anesthesiaPatientPreparationInMinutes, 120, 60));

        Assert.Equal("Error: The anesthesia/preparation duration must be more than 0 minutes.", anesthesiaException.Message);

        var surgeryException = Assert.Throws<BusinessRuleValidationException>(() =>
            new OperationTypeDuration(30, surgeryInMinutes, 60));

        Assert.Equal("Error: The surgery duration must be more than 0 minutes.", surgeryException.Message);

        var cleaningException = Assert.Throws<BusinessRuleValidationException>(() =>
            new OperationTypeDuration(30, 120, cleaningInMinutes));

        Assert.Equal("Error: The cleaning duration must be more than 0 minutes.", cleaningException.Message);
    }
}
