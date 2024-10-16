using System;
using HealthcareApp.Domain.OperationTypes;
using HealthcareApp.Domain.Shared;
using Moq;
using Xunit;
using HealthcareApp.Infraestructure.OperationTypes;
using HealthcareApp.Infraestructure;
using Microsoft.EntityFrameworkCore;
using HealthcareApp.Domain.Specializations;

namespace HealthcareApp.Tests;

public class OperationTypeRepositoryMockTests
{
    private readonly Mock<IOperationTypeRepository> _mockOperationTypeRepository;


    public OperationTypeRepositoryMockTests()
    {
        _mockOperationTypeRepository = new Mock<IOperationTypeRepository>();
    }

    [Fact]
    public async Task TestOperationTypeNameExistsWithMock()
    {
        var operationTypeName = "Operation Type A";
        _mockOperationTypeRepository.Setup(repo => repo.OperationTypeNameExists(operationTypeName))
            .ReturnsAsync(true);

        var exists = await _mockOperationTypeRepository.Object.OperationTypeNameExists(operationTypeName);

        Assert.True(exists);
        _mockOperationTypeRepository.Verify(repo => repo.OperationTypeNameExists(operationTypeName), Times.Once);
    }

    [Fact]
    public async Task TestOperationTypeNameDoesNotExistWithMock()
    {
        var operationTypeName = "NonExistentOperationType";
        _mockOperationTypeRepository.Setup(repo => repo.OperationTypeNameExists(operationTypeName))
            .ReturnsAsync(false);

        var exists = await _mockOperationTypeRepository.Object.OperationTypeNameExists(operationTypeName);

        Assert.False(exists);
        _mockOperationTypeRepository.Verify(repo => repo.OperationTypeNameExists(operationTypeName), Times.Once);
    }

    [Fact]
    public async Task TestGetAllWithDetailsAsync()
    {
        var operationTypes = new List<OperationType>
        {
            new OperationType(new OperationTypeName("Surgery"), new OperationTypeDuration(30, 90, 15), new List<OperationTypeRequiredStaff>
            {
                new OperationTypeRequiredStaff(new Specialization(new SpecializationName("Surgeon")), 2)
            }),
            new OperationType(new OperationTypeName("Anesthesia"), new OperationTypeDuration(15, 30, 5), new List<OperationTypeRequiredStaff>
            {
                new OperationTypeRequiredStaff(new Specialization(new SpecializationName("Anesthesiologist")), 5),
                new OperationTypeRequiredStaff(new Specialization(new SpecializationName("Surgeon")), 1)
            })
        };

        _mockOperationTypeRepository.Setup(repo => repo.GetAllWithDetailsAsync())
            .ReturnsAsync(operationTypes);

        var result = await _mockOperationTypeRepository.Object.GetAllWithDetailsAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);

        var surgery = result[0];
        Assert.Equal("Surgery", surgery.Name.Name);
        Assert.Equal(30, surgery.Duration.AnesthesiaPatientPreparationInMinutes.TotalMinutes);
        Assert.Equal(90, surgery.Duration.SurgeryInMinutes.TotalMinutes);
        Assert.Equal(15, surgery.Duration.CleaningInMinutes.TotalMinutes);
        Assert.Single(surgery.RequiredStaff);
        Assert.Equal("Surgeon", surgery.RequiredStaff[0].Specialization.Name.Name);
        Assert.Equal(2, surgery.RequiredStaff[0].Total);

        var anesthesia = result[1];
        Assert.Equal("Anesthesia", anesthesia.Name.Name);
        Assert.Equal(15, anesthesia.Duration.AnesthesiaPatientPreparationInMinutes.TotalMinutes);
        Assert.Equal(30, anesthesia.Duration.SurgeryInMinutes.TotalMinutes);
        Assert.Equal(5, anesthesia.Duration.CleaningInMinutes.TotalMinutes);
        Assert.Equal(2, anesthesia.RequiredStaff.Count);

        Assert.Equal("Anesthesiologist", anesthesia.RequiredStaff[0].Specialization.Name.Name);
        Assert.Equal(5, anesthesia.RequiredStaff[0].Total);

        Assert.Equal("Surgeon", anesthesia.RequiredStaff[1].Specialization.Name.Name);
        Assert.Equal(1, anesthesia.RequiredStaff[1].Total);

        _mockOperationTypeRepository.Verify(repo => repo.GetAllWithDetailsAsync(), Times.Once);
    }


    [Fact]
    public async Task TestGetByIdWithDetailsAsync_ShouldReturnOperationType()
    {

        var operationType = new OperationType(new OperationTypeName("Surgery"), new OperationTypeDuration(30, 60, 15), new List<OperationTypeRequiredStaff>
            {
                new OperationTypeRequiredStaff(new Specialization(new SpecializationName("Surgeon")), 2)
            });

        var operationTypeId = operationType.Id;

        _mockOperationTypeRepository.Setup(repo => repo.GetByIdWithDetailsAsync(operationTypeId))
            .ReturnsAsync(operationType);

        var result = await _mockOperationTypeRepository.Object.GetByIdWithDetailsAsync(operationTypeId);

        Assert.NotNull(result);
        Assert.Equal(operationTypeId, result.Id);
        Assert.Equal("Surgery", result.Name.Name);
        _mockOperationTypeRepository.Verify(repo => repo.GetByIdWithDetailsAsync(operationTypeId), Times.Once);
    }

    [Fact]
    public async Task TestGetByIdWithDetailsAsync_ShouldReturnNull()
    {
        var operationTypeId = new OperationTypeId(Guid.NewGuid());

        _mockOperationTypeRepository.Setup(repo => repo.GetByIdWithDetailsAsync(operationTypeId))
            .ReturnsAsync((OperationType)null);

        var result = await _mockOperationTypeRepository.Object.GetByIdWithDetailsAsync(operationTypeId);

        Assert.Null(result);
        _mockOperationTypeRepository.Verify(repo => repo.GetByIdWithDetailsAsync(operationTypeId), Times.Once);
    }
}
