using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backoffice.Domain.Shared;
using Backoffice.Domain.Specializations;
using Xunit;

namespace Backoffice.Tests
{
    public class SpecializationServiceTests
    {
        private readonly SpecializationMapper _mapper = new SpecializationMapper();

        private SpecializationService Setup(List<Specialization> specializationDatabase)
        {
            var specializationRepository = new Mock<ISpecializationRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();

            // Mock GetAllAsync
            specializationRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(specializationDatabase);

            // Mock GetByIdAsync
            specializationRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<SpecializationId>()))
                .ReturnsAsync((SpecializationId id) => specializationDatabase.SingleOrDefault(spec => spec.Id.Equals(id)));

            // Mock AddAsync
            specializationRepository.Setup(repo => repo.AddAsync(It.IsAny<Specialization>()))
                .Callback<Specialization>(spec => specializationDatabase.Add(spec));

            // Mock SpecializationNameExists
            specializationRepository.Setup(repo => repo.SpecializationNameExists(It.IsAny<string>()))
                .ReturnsAsync((string name) => specializationDatabase.Any(spec => spec.Name.Name == name));

            unitOfWork.Setup(uow => uow.CommitAsync()).ReturnsAsync(0);

            return new SpecializationService(unitOfWork.Object, specializationRepository.Object, _mapper);
        }

        [Fact]
        public async Task GetAllAsync_ExistingSpecializations()
        {
            var specializationDatabase = new List<Specialization>
            {
                _mapper.ToDomainForTests("Surgeon"),
                _mapper.ToDomainForTests("Cardio")
            };
            var service = Setup(specializationDatabase);

            var result = await service.GetAllAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            Assert.Equal("Surgeon", result[0].Name);
            Assert.Equal("Cardio", result[1].Name);
        }

        [Fact]
        public async Task GetByIdAsync_SpecializationExists()
        {
            var specializationDatabase = new List<Specialization>
            {
                _mapper.ToDomainForTests("Surgeon"),
                _mapper.ToDomainForTests("Cardio")
            };
            var service = Setup(specializationDatabase);

            var specialization = await service.GetByIdAsync(specializationDatabase[1].Id.AsGuid());

            Assert.NotNull(specialization);
            Assert.Equal("Cardio", specialization.Name);
        }

        [Fact]
        public async Task GetByIdAsync_SpecializationDoesNotExist()
        {
            var specializationDatabase = new List<Specialization>
            {
                _mapper.ToDomainForTests("Surgeon")
            };
            var service = Setup(specializationDatabase);

            var result = await service.GetByIdAsync(Guid.NewGuid());

            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_WithValidData()
        {
            var specializationDatabase = new List<Specialization>();
            var service = Setup(specializationDatabase);

            var dto = _mapper.ToCreateDtoForTests("Surgeon");

            var result = await service.AddAsync(dto);

            Assert.NotNull(result);
            Assert.Single(specializationDatabase);
            Assert.Equal("Surgeon", result.Name);
        }

        [Fact]
        public async Task AddAsync_WithDuplicateName()
        {
            var specializationDatabase = new List<Specialization>
            {
                _mapper.ToDomainForTests("Surgeon")
            };
            var service = Setup(specializationDatabase);

            var dto = _mapper.ToCreateDtoForTests("Surgeon");

            var exception = await Assert.ThrowsAsync<BusinessRuleValidationException>(() => service.AddAsync(dto));

            Assert.Equal("Error: This specialization name is already being used.", exception.Message);
            Assert.Single(specializationDatabase);
        }
    }
}
