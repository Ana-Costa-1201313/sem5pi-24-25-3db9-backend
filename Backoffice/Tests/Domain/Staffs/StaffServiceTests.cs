using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backoffice.Domain.Shared;
using Backoffice.Domain.Staffs;
using Xunit;

namespace Backoffice.Tests
{
    public class StaffServiceTests
    {
        private StaffService Setup(List<Staff> staffDb)
        {
            var staffRepository = new Mock<IStaffRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();

            staffRepository.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(staffDb);

            staffRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<StaffId>()))
            .ReturnsAsync((StaffId id) => staffDb.SingleOrDefault(s => s.Id.Equals(id)));

            staffRepository.Setup(repo => repo.AddAsync(It.IsAny<Staff>()))
            .Callback<Staff>(s => staffDb.Add(s));

            staffRepository.Setup(repo => repo.GetLastMechanographicNumAsync())
            .ReturnsAsync(staffDb.DefaultIfEmpty().Max(s => s != null ? s.MecNumSequence : 0));

            staffRepository.Setup(repo => repo.GetStaffByEmailAsync(It.IsAny<Email>()))
            .ReturnsAsync((Email email) => staffDb.FirstOrDefault(s => s.Email == email));

            unitOfWork.Setup(uow => uow.CommitAsync()).ReturnsAsync(0);

            var staffMapper = new StaffMapper();

            return new StaffService(unitOfWork.Object, staffRepository.Object, staffMapper);
        }

        [Fact]
        public async Task GetAllAsync()
        {
            var staffDb = new List<Staff>();
            var service = Setup(staffDb);

            List<string> AvailabilitySlots = new List<string>();
            AvailabilitySlots.Add("2024 - 10 - 10T12: 00:00 / 2024 - 10 - 11T15: 00:00");
            AvailabilitySlots.Add("2024 - 10 - 14T12: 00:00 / 2024 - 10 - 19T15: 00:00");

            CreateStaffDto dto1 = new CreateStaffDto
            {
                FirstName = "ana",
                LastName = "costa",
                FullName = "ana costa",
                LicenseNumber = 1,
                Phone = "999999999",
                Specialization = "spec",
                AvailabilitySlots = AvailabilitySlots,
                Role = Role.Nurse,
                RecruitmentYear = 2024
            };

            CreateStaffDto dto2 = new CreateStaffDto
            {
                FirstName = "maria",
                LastName = "silva",
                FullName = "maria silva",
                LicenseNumber = 2,
                Phone = "999999989",
                Specialization = "spec",
                AvailabilitySlots = AvailabilitySlots,
                Role = Role.Doctor,
                RecruitmentYear = 2024
            };

            var staff1 = new Staff(dto1, 1);
            var staff2 = new Staff(dto2, 2);

            staffDb.Add(staff1);
            staffDb.Add(staff2);

            var result = await service.GetAllAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);


            Assert.Equal("ana", result[0].FirstName);
            Assert.Equal("costa", result[0].LastName);
            Assert.Equal("ana costa", result[0].FullName);
            Assert.Equal(1, result[0].LicenseNumber);
            Assert.Equal("999999999", result[0].Phone);
            Assert.Equal("spec", result[0].Specialization);
            Assert.Equal(Role.Nurse, result[0].Role);

            Assert.Equal("maria", result[1].FirstName);
            Assert.Equal("silva", result[1].LastName);
            Assert.Equal("maria silva", result[1].FullName);
            Assert.Equal(2, result[1].LicenseNumber);
            Assert.Equal("999999989", result[1].Phone);
            Assert.Equal("spec", result[1].Specialization);
            Assert.Equal(Role.Doctor, result[1].Role);

        }

        [Fact]
        public async Task GetByIdAsync()
        {
            var staffDb = new List<Staff>();
            var service = Setup(staffDb);

            List<string> AvailabilitySlots = new List<string>();
            AvailabilitySlots.Add("2024 - 10 - 10T12: 00:00 / 2024 - 10 - 11T15: 00:00");
            AvailabilitySlots.Add("2024 - 10 - 14T12: 00:00 / 2024 - 10 - 19T15: 00:00");

            CreateStaffDto dto1 = new CreateStaffDto
            {
                FirstName = "ana",
                LastName = "costa",
                FullName = "ana costa",
                LicenseNumber = 1,
                Phone = "999999999",
                Specialization = "spec",
                AvailabilitySlots = AvailabilitySlots,
                Role = Role.Nurse,
                RecruitmentYear = 2024
            };

            CreateStaffDto dto2 = new CreateStaffDto
            {
                FirstName = "maria",
                LastName = "silva",
                FullName = "maria silva",
                LicenseNumber = 2,
                Phone = "999999989",
                Specialization = "spec",
                AvailabilitySlots = AvailabilitySlots,
                Role = Role.Doctor,
                RecruitmentYear = 2024
            };

            var staff1 = new Staff(dto1, 1);
            var staff2 = new Staff(dto2, 2);

            staffDb.Add(staff1);
            staffDb.Add(staff2);

            var result = await service.GetByIdAsync(staff1.Id.AsGuid());

            Assert.NotNull(result);

            Assert.Equal("ana", result.FirstName);
            Assert.Equal("costa", result.LastName);
            Assert.Equal("ana costa", result.FullName);
            Assert.Equal(1, result.LicenseNumber);
            Assert.Equal("999999999", result.Phone);
            Assert.Equal("spec", result.Specialization);
            Assert.Equal(Role.Nurse, result.Role);
        }

        [Fact]
        public async Task InvalidGetByIdAsync()
        {
            var staffDb = new List<Staff>();
            var service = Setup(staffDb);

            List<string> AvailabilitySlots = new List<string>();
            AvailabilitySlots.Add("2024 - 10 - 10T12: 00:00 / 2024 - 10 - 11T15: 00:00");
            AvailabilitySlots.Add("2024 - 10 - 14T12: 00:00 / 2024 - 10 - 19T15: 00:00");

            CreateStaffDto dto1 = new CreateStaffDto
            {
                FirstName = "ana",
                LastName = "costa",
                FullName = "ana costa",
                LicenseNumber = 1,
                Phone = "999999999",
                Specialization = "spec",
                AvailabilitySlots = AvailabilitySlots,
                Role = Role.Nurse,
                RecruitmentYear = 2024
            };

            CreateStaffDto dto2 = new CreateStaffDto
            {
                FirstName = "maria",
                LastName = "silva",
                FullName = "maria silva",
                LicenseNumber = 2,
                Phone = "999999989",
                Specialization = "spec",
                AvailabilitySlots = AvailabilitySlots,
                Role = Role.Doctor,
                RecruitmentYear = 2024
            };

            var staff1 = new Staff(dto1, 1);
            var staff2 = new Staff(dto2, 2);

            staffDb.Add(staff1);

            var result = await service.GetByIdAsync(staff2.Id.AsGuid());

            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync()
        {
            var staffDb = new List<Staff>();
            var service = Setup(staffDb);

            List<string> AvailabilitySlots = new List<string>();
            AvailabilitySlots.Add("2024 - 10 - 10T12: 00:00 / 2024 - 10 - 11T15: 00:00");
            AvailabilitySlots.Add("2024 - 10 - 14T12: 00:00 / 2024 - 10 - 19T15: 00:00");

            CreateStaffDto dto1 = new CreateStaffDto
            {
                FirstName = "ana",
                LastName = "costa",
                FullName = "ana costa",
                LicenseNumber = 1,
                Phone = "999999999",
                Specialization = "spec",
                AvailabilitySlots = AvailabilitySlots,
                Role = Role.Nurse,
                RecruitmentYear = 2024
            };

            var result = await service.AddAsync(dto1);

            Assert.NotNull(result);

            Assert.Equal("ana", result.FirstName);
            Assert.Equal("costa", result.LastName);
            Assert.Equal("ana costa", result.FullName);
            Assert.Equal(1, result.LicenseNumber);
            Assert.Equal("999999999", result.Phone);
            Assert.Equal("spec", result.Specialization);
            Assert.Equal(Role.Nurse, result.Role);
        }

        [Fact]
        public async Task Deactivate()
        {
            var staffDb = new List<Staff>();
            var service = Setup(staffDb);

            List<string> AvailabilitySlots = new List<string>();
            AvailabilitySlots.Add("2024 - 10 - 10T12: 00:00 / 2024 - 10 - 11T15: 00:00");
            AvailabilitySlots.Add("2024 - 10 - 14T12: 00:00 / 2024 - 10 - 19T15: 00:00");

            CreateStaffDto dto1 = new CreateStaffDto
            {
                FirstName = "ana",
                LastName = "costa",
                FullName = "ana costa",
                LicenseNumber = 1,
                Phone = "999999999",
                Specialization = "spec",
                AvailabilitySlots = AvailabilitySlots,
                Role = Role.Nurse,
                RecruitmentYear = 2024
            };

            var staff1 = new Staff(dto1, 1);

            staffDb.Add(staff1);

            var result = await service.Deactivate(staff1.Id.AsGuid());

            Assert.NotNull(result);

            Assert.Equal("Deactivated Staff", result.FirstName);
            Assert.Equal("Deactivated Staff", result.LastName);
            Assert.Equal("Deactivated Staff", result.FullName);
            Assert.Null(result.Phone);
            Assert.Equal("Deactivated Staff", result.Specialization);
            Assert.False(result.Active);
        }
    }
}