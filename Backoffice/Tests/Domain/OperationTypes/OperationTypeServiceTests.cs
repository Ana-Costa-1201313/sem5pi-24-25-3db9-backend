using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backoffice.Domain.OperationTypes;
using Backoffice.Domain.Shared;
using Backoffice.Domain.Specializations;
using Xunit;
using Backoffice.Domain.Logs;

namespace Backoffice.Tests
{
    public class OperationTypeServiceTests
    {
        private OperationTypeService Setup(List<OperationType> operationTypesDatabase, List<Specialization> specializationsDatabase)
        {
            var operationTypeRepository = new Mock<IOperationTypeRepository>();
            var specializationRepository = new Mock<ISpecializationRepository>();
            var logRepository = new Mock<ILogRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();

            // Mock GetAllWithDetailsAsync
            operationTypeRepository.Setup(repo => repo.GetAllWithDetailsAsync()).ReturnsAsync(operationTypesDatabase);

            // Mock GetByIdWithDetailsAsync
            operationTypeRepository.Setup(repo => repo.GetByIdWithDetailsAsync(It.IsAny<OperationTypeId>()))
                .ReturnsAsync((OperationTypeId id) => operationTypesDatabase.SingleOrDefault(op => op.Id.Equals(id)));

            // Mock AddAsync
            operationTypeRepository.Setup(repo => repo.AddAsync(It.IsAny<OperationType>()))
                .Callback<OperationType>(op => operationTypesDatabase.Add(op));

            // Mock OperationTypeNameExists
            operationTypeRepository.Setup(repo => repo.OperationTypeNameExists(It.IsAny<string>()))
                .ReturnsAsync((string name) => operationTypesDatabase.Any(op => op.Name.Name == name));


            // Mock SpecializationNameExists
            specializationRepository.Setup(repo => repo.SpecializationNameExists(It.IsAny<string>()))
                .ReturnsAsync((string name) => specializationsDatabase.Any(spec => spec.Name.Name == name));

            // Mock GetBySpecializationName
            specializationRepository.Setup(repo => repo.GetBySpecializationName(It.IsAny<string>()))
                .ReturnsAsync((string name) => specializationsDatabase.SingleOrDefault(spec => spec.Name.Name == name));

            unitOfWork.Setup(uow => uow.CommitAsync()).ReturnsAsync(0);

            return new OperationTypeService(unitOfWork.Object, operationTypeRepository.Object, specializationRepository.Object, logRepository.Object);
        }

        [Fact]
        public async Task GetAllAsync_ExistingOperationTypes()
        {
            var operationTypesDatabase = new List<OperationType>();
            var specializationsDatabase = new List<Specialization>();
            var service = Setup(operationTypesDatabase, specializationsDatabase);

            var requiredStaff1 = new List<(string SpecializationName, int Total)>
                {
                    ("Surgeon", 5)
                };
            var operationType1 = OperationTypeMapper.ToDomainForTests("Surgery", 30, 60, 15, requiredStaff1);

            var requiredStaff2 = new List<(string SpecializationName, int Total)>
                {
                    ("Surgeon", 2),
                    ("Cardio", 3)
                };
            var operationType2 = OperationTypeMapper.ToDomainForTests("Embolectomy", 30, 60, 15, requiredStaff2);


            operationTypesDatabase.Add(operationType1);
            operationTypesDatabase.Add(operationType2);

            var result = await service.GetAllAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            Assert.Equal("Surgery", result[0].Name);
            Assert.Equal(30, result[0].AnesthesiaPatientPreparationInMinutes);
            Assert.Equal(60, result[0].SurgeryInMinutes);
            Assert.Equal(15, result[0].CleaningInMinutes);
            Assert.Equal("Surgeon", result[0].RequiredStaff[0].Specialization);
            Assert.Equal(5, result[0].RequiredStaff[0].Total);


            Assert.Equal("Embolectomy", result[1].Name);
            Assert.Equal(30, result[1].AnesthesiaPatientPreparationInMinutes);
            Assert.Equal(60, result[1].SurgeryInMinutes);
            Assert.Equal(15, result[1].CleaningInMinutes);
            Assert.Equal("Surgeon", result[1].RequiredStaff[0].Specialization);
            Assert.Equal(2, result[1].RequiredStaff[0].Total);
            Assert.Equal("Cardio", result[1].RequiredStaff[1].Specialization);
            Assert.Equal(3, result[1].RequiredStaff[1].Total);
        }

        [Fact]
        public async Task GetByIdAsync_OperationTypeExists()
        {
            var operationTypesDatabase = new List<OperationType>();
            var specializationsDatabase = new List<Specialization>();
            var service = Setup(operationTypesDatabase, specializationsDatabase);

            var requiredStaff1 = new List<(string SpecializationName, int Total)>
                {
                    ("Surgeon", 5)
                };
            var operationType1 = OperationTypeMapper.ToDomainForTests("Surgery", 30, 60, 15, requiredStaff1);

            var requiredStaff2 = new List<(string SpecializationName, int Total)>
                {
                    ("Surgeon", 2),
                    ("Cardio", 3)
                };
            var operationType2 = OperationTypeMapper.ToDomainForTests("Embolectomy", 30, 60, 15, requiredStaff2);

            operationTypesDatabase.Add(operationType1);
            operationTypesDatabase.Add(operationType2);

            var result = await service.GetByIdAsync(operationType2.Id.AsGuid());

            Assert.NotNull(result);
            Assert.Equal("Embolectomy", result.Name);
            Assert.Equal(30, result.AnesthesiaPatientPreparationInMinutes);
            Assert.Equal(60, result.SurgeryInMinutes);
            Assert.Equal(15, result.CleaningInMinutes);
            Assert.Equal("Surgeon", result.RequiredStaff[0].Specialization);
            Assert.Equal(2, result.RequiredStaff[0].Total);
            Assert.Equal("Cardio", result.RequiredStaff[1].Specialization);
            Assert.Equal(3, result.RequiredStaff[1].Total);
        }

        [Fact]
        public async Task GetByIdAsync_OperationTypeDoesntExists()
        {
            var operationTypesDatabase = new List<OperationType>();
            var specializationsDatabase = new List<Specialization>();
            var service = Setup(operationTypesDatabase, specializationsDatabase);

            var requiredStaff1 = new List<(string SpecializationName, int Total)>
                {
                    ("Surgeon", 5)
                };
            var operationType1 = OperationTypeMapper.ToDomainForTests("Surgery", 30, 60, 15, requiredStaff1);

            var requiredStaff2 = new List<(string SpecializationName, int Total)>
                {
                    ("Surgeon", 2),
                    ("Cardio", 3)
                };
            var operationType2 = OperationTypeMapper.ToDomainForTests("Embolectomy", 30, 60, 15, requiredStaff2);

            operationTypesDatabase.Add(operationType1);

            var result = await service.GetByIdAsync(operationType2.Id.AsGuid());

            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_WithValidData()
        {

            var operationTypesDatabase = new List<OperationType>();
            var specializationsDatabase = new List<Specialization>
            {
                new Specialization(new SpecializationName("Surgeon"))
            };
            var service = Setup(operationTypesDatabase, specializationsDatabase);


            var requiredStaff = new List<(string SpecializationName, int Total)>
                {
                    ("Surgeon", 2)
                };
            var dto = OperationTypeMapper.ToCreateDtoForTests("Surgery", 30, 60, 15, requiredStaff);


            var result = await service.AddAsync(dto);

            Assert.NotNull(result);
            Assert.Single(operationTypesDatabase);
            Assert.Equal("Surgery", result.Name);
            Assert.Equal(30, result.AnesthesiaPatientPreparationInMinutes);
            Assert.Equal(60, result.SurgeryInMinutes);
            Assert.Equal(15, result.CleaningInMinutes);
            Assert.Equal("Surgeon", result.RequiredStaff[0].Specialization);
            Assert.Equal(2, result.RequiredStaff[0].Total);
        }

        [Fact]
        public async Task AddAsync_WithInvalidSpecializationName()
        {

            var operationTypesDatabase = new List<OperationType>();
            var specializationsDatabase = new List<Specialization>
            {
                new Specialization(new SpecializationName("Surgeon"))
            };
            var service = Setup(operationTypesDatabase, specializationsDatabase);


            var requiredStaff = new List<(string SpecializationName, int Total)>
                {
                    ("Cardio", 2)
                };
            var dto = OperationTypeMapper.ToCreateDtoForTests("Surgery", 30, 60, 15, requiredStaff);


            var exception = await Assert.ThrowsAsync<BusinessRuleValidationException>(() => service.AddAsync(dto));

            Assert.Equal("Error: There is no specialization with the name Cardio.", exception.Message);
            Assert.Empty(operationTypesDatabase);
        }

        [Fact]
        public async Task AddAsync_WithDuplicateSpecialization()
        {

            var operationTypesDatabase = new List<OperationType>();
            var specializationsDatabase = new List<Specialization>
            {
                new Specialization(new SpecializationName("Cardio"))
            };
            var service = Setup(operationTypesDatabase, specializationsDatabase);

            var requiredStaff = new List<(string SpecializationName, int Total)>
                {
                    ("Cardio", 2),
                    ("Cardio", 2)
                };
            var dto = OperationTypeMapper.ToCreateDtoForTests("Surgery", 30, 60, 15, requiredStaff);

            var exception = await Assert.ThrowsAsync<BusinessRuleValidationException>(() => service.AddAsync(dto));

            Assert.Equal("Error: Can't have duplicate specializations -> Cardio.", exception.Message);
            Assert.Empty(operationTypesDatabase);
        }

        [Fact]
        public async Task AddAsync_WithDuplicateOperationName()
        {

            var operationTypesDatabase = new List<OperationType>();
            var specializationsDatabase = new List<Specialization>
            {
                new Specialization(new SpecializationName("Surgeon"))
            };
            var service = Setup(operationTypesDatabase, specializationsDatabase);

            var requiredStaff1 = new List<(string SpecializationName, int Total)>
                {
                    ("Surgeon", 2)
                };
            var dto1 = OperationTypeMapper.ToCreateDtoForTests("Surgery", 30, 60, 15, requiredStaff1);


            var requiredStaff2 = new List<(string SpecializationName, int Total)>
                {
                    ("Surgeon", 2)
                };
            var dto2 = OperationTypeMapper.ToCreateDtoForTests("Surgery", 30, 60, 15, requiredStaff2);


            await service.AddAsync(dto1);
            var exception = await Assert.ThrowsAsync<BusinessRuleValidationException>(() => service.AddAsync(dto2));

            Assert.Equal("Error: This operation type name is already being used.", exception.Message);
            Assert.Single(operationTypesDatabase);
        }


    }
}