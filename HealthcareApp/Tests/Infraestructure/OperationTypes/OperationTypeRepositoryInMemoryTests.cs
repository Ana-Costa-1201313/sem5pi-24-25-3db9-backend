using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthcareApp.Domain.OperationTypes;
using HealthcareApp.Domain.Shared;
using HealthcareApp.Domain.Specializations;
using HealthcareApp.Infraestructure;
using HealthcareApp.Infraestructure.Categories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HealthcareApp.Tests
{
    public class OperationTypeRepositoryInMemoryTests
    {
        private readonly OperationTypeRepository _operationTypeRepository;
        private readonly BDContext _context;

        public OperationTypeRepositoryInMemoryTests()
        {
            var options = new DbContextOptionsBuilder<BDContext>()
                .UseInMemoryDatabase(databaseName: "HealthcareAppTestDb")
                .Options;

            _context = new BDContext(options);
            _operationTypeRepository = new OperationTypeRepository(_context);
        }

        [Fact]
        public async Task TestOperationTypeNameExistsWithDBInMemory()
        {
            _context.OperationTypes.RemoveRange(_context.OperationTypes);
            await _context.SaveChangesAsync();

            var operationType = new OperationType(new OperationTypeName("Surgery"),
                new OperationTypeDuration(30, 60, 15),
                new List<OperationTypeRequiredStaff>
                {
                    new OperationTypeRequiredStaff(new Specialization(new SpecializationName("Surgeon")), 2)
                });

            _context.OperationTypes.Add(operationType);
            await _context.SaveChangesAsync();

            var exists = await _operationTypeRepository.OperationTypeNameExists("Surgery");

            Assert.True(exists);
        }

        [Fact]
        public async Task TestOperationTypeNameDoesNotExistWithDBInMemory()
        {
            _context.OperationTypes.RemoveRange(_context.OperationTypes);
            await _context.SaveChangesAsync();

            var operationType = new OperationType(new OperationTypeName("Surgery"),
                new OperationTypeDuration(30, 60, 15),
                new List<OperationTypeRequiredStaff>
                {
                    new OperationTypeRequiredStaff(new Specialization(new SpecializationName("Surgeon")), 2)
                });

            _context.OperationTypes.Add(operationType);
            await _context.SaveChangesAsync();

            var exists = await _operationTypeRepository.OperationTypeNameExists("NonExistentOperationType");

            Assert.False(exists);
        }

        [Fact]
        public async Task TestGetOperationTypeByIdFoundWithDBInMemory()
        {
            _context.OperationTypes.RemoveRange(_context.OperationTypes);
            await _context.SaveChangesAsync();

            var operationType = new OperationType(new OperationTypeName("Surgery"),
                new OperationTypeDuration(30, 60, 15),
                new List<OperationTypeRequiredStaff>
                {
            new OperationTypeRequiredStaff(new Specialization(new SpecializationName("Surgeon")), 2)
                });

            _context.OperationTypes.Add(operationType);
            await _context.SaveChangesAsync();

            var result = await _operationTypeRepository.GetByIdWithDetailsAsync(operationType.Id);

            Assert.NotNull(result);
            Assert.Equal("Surgery", result.Name.Name);
            Assert.NotNull(result.Duration);
            Assert.Equal(30, result.Duration.AnesthesiaPatientPreparationInMinutes.TotalMinutes);
            Assert.Equal(60, result.Duration.SurgeryInMinutes.TotalMinutes);
            Assert.Equal(15, result.Duration.CleaningInMinutes.TotalMinutes);

            Assert.NotNull(result.RequiredStaff);
            Assert.Single(result.RequiredStaff);

            Assert.Equal("Surgeon", result.RequiredStaff[0].Specialization.Name.Name);
            Assert.Equal(2, result.RequiredStaff[0].Total);

            Assert.True(result.Active);
        }


        [Fact]
        public async Task TestGetOperationTypeByIdNotFoundWithDBInMemory()
        {

            _context.OperationTypes.RemoveRange(_context.OperationTypes);
            await _context.SaveChangesAsync();

            var operationTypeId = new OperationTypeId(Guid.NewGuid());

            var result = await _operationTypeRepository.GetByIdWithDetailsAsync(operationTypeId);

            Assert.Null(result);
        }

        [Fact]
        public async Task TestGetAllWithDetailsWithDataWithDBInMemory()
        {
            _context.OperationTypes.RemoveRange(_context.OperationTypes);
            await _context.SaveChangesAsync();

            var operationTypes = new List<OperationType>
            {
                new OperationType(new OperationTypeName("Surgery"),
                    new OperationTypeDuration(30, 60, 15),
                    new List<OperationTypeRequiredStaff>
                    {
                        new OperationTypeRequiredStaff(new Specialization(new SpecializationName("Surgeon")), 2)
                    }),
                new OperationType(new OperationTypeName("Anesthesia"),
                    new OperationTypeDuration(15, 30, 5),
                    new List<OperationTypeRequiredStaff>
                    {
                        new OperationTypeRequiredStaff(new Specialization(new SpecializationName("Anesthesiologist")), 5)
                    })
            };

            _context.OperationTypes.AddRange(operationTypes);
            await _context.SaveChangesAsync();

            var result = await _operationTypeRepository.GetAllWithDetailsAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            var surgeryOperation = result.FirstOrDefault(op => op.Name.Name == "Surgery");
            Assert.NotNull(surgeryOperation);
            Assert.Equal("Surgery", surgeryOperation.Name.Name);
            Assert.NotNull(surgeryOperation.Duration);
            Assert.Equal(30, surgeryOperation.Duration.AnesthesiaPatientPreparationInMinutes.TotalMinutes);
            Assert.Equal(60, surgeryOperation.Duration.SurgeryInMinutes.TotalMinutes);
            Assert.Equal(15, surgeryOperation.Duration.CleaningInMinutes.TotalMinutes);
            Assert.NotNull(surgeryOperation.RequiredStaff);
            Assert.Single(surgeryOperation.RequiredStaff);
            Assert.Equal("Surgeon", surgeryOperation.RequiredStaff[0].Specialization.Name.Name);
            Assert.Equal(2, surgeryOperation.RequiredStaff[0].Total);
            Assert.True(surgeryOperation.Active);


            var anesthesiaOperation = result.FirstOrDefault(op => op.Name.Name == "Anesthesia");
            Assert.NotNull(anesthesiaOperation);
            Assert.Equal("Anesthesia", anesthesiaOperation.Name.Name);
            Assert.NotNull(anesthesiaOperation.Duration);
            Assert.Equal(15, anesthesiaOperation.Duration.AnesthesiaPatientPreparationInMinutes.TotalMinutes);
            Assert.Equal(30, anesthesiaOperation.Duration.SurgeryInMinutes.TotalMinutes);
            Assert.Equal(5, anesthesiaOperation.Duration.CleaningInMinutes.TotalMinutes);
            Assert.NotNull(anesthesiaOperation.RequiredStaff);
            Assert.Single(anesthesiaOperation.RequiredStaff);
            Assert.Equal("Anesthesiologist", anesthesiaOperation.RequiredStaff[0].Specialization.Name.Name);
            Assert.Equal(5, anesthesiaOperation.RequiredStaff[0].Total);
            Assert.True(anesthesiaOperation.Active);
        }

        [Fact]
        public async Task TestGetAllWithDetailsWithoutDataWithDBInMemory()
        {
            _context.OperationTypes.RemoveRange(_context.OperationTypes);
            await _context.SaveChangesAsync();

            var result = await _operationTypeRepository.GetAllWithDetailsAsync();

            Assert.Empty(result);
        }
    }
}
