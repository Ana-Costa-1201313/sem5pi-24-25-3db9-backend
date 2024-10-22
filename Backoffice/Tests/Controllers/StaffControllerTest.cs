using Backoffice.Controllers;
using Backoffice.Domain.Shared;
using Backoffice.Domain.Staffs;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Backoffice.Tests
{
    public class StaffControllerTest
    {
        private readonly Mock<IStaffRepository> _repo;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly StaffService _service;
        private readonly StaffController _controller;

        public StaffControllerTest()
        {
            _repo = new Mock<IStaffRepository>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _service = new StaffService(_unitOfWork.Object, _repo.Object, new StaffMapper());
            _controller = new StaffController(_service);
        }

        [Fact]
        public async Task GetAllStaff()
        {
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

            var expectedList = new List<Staff> { staff1, staff2 };

            _repo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedList);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<ActionResult<IEnumerable<StaffDto>>>(result);

            var objectResult = Assert.IsType<OkObjectResult>(okResult.Result);

            var actualList = Assert.IsAssignableFrom<IEnumerable<StaffDto>>(objectResult.Value);

            Assert.Equal(expectedList.Count, actualList.Count());
        }

        [Fact]
        public async Task GetAllEmpty()
        {
            var expectedList = new List<Staff>();

            _repo.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(expectedList);

            var result = await _controller.GetAll();

            var noContentResult = Assert.IsType<ActionResult<IEnumerable<StaffDto>>>(result);

            Assert.IsType<NoContentResult>(noContentResult.Result);
        }

        [Fact]
        public async Task GetByIdStaff()
        {
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

            var staff = new Staff(dto1, 1);

            _repo.Setup(repo => repo.GetByIdAsync(It.IsAny<StaffId>())).ReturnsAsync(staff);

            var result = await _controller.GetById(staff.Id.AsGuid());

            var okResult = Assert.IsType<ActionResult<StaffDto>>(result);

            var objectResult = Assert.IsType<OkObjectResult>(okResult.Result);

            var actualStaff = Assert.IsType<StaffDto>(objectResult.Value);

            Assert.Equal(staff.FirstName, actualStaff.FirstName);
            Assert.Equal(staff.LastName, actualStaff.LastName);
            Assert.Equal(staff.FullName, actualStaff.FullName);
            Assert.Equal(staff.LicenseNumber.LicenseNum, actualStaff.LicenseNumber);
            Assert.Equal(staff.Specialization, actualStaff.Specialization);
            Assert.Equal(staff.Role, actualStaff.Role);
        }

        [Fact]
        public async Task GetByIdEmpty()
        {
            var staffId = Guid.NewGuid();

            _repo.Setup(repo => repo.GetByIdAsync(It.IsAny<StaffId>())).ReturnsAsync((Staff)null);

            var result = await _controller.GetById(staffId);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateStaff()
        {
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

            StaffDto createdStaff = new StaffDto
            {
                Id = Guid.NewGuid(),
                FirstName = "ana",
                LastName = "costa",
                FullName = "ana costa",
                LicenseNumber = 1,
                Phone = "999999999",
                Specialization = "spec",
                AvailabilitySlots = AvailabilitySlots,
                Role = Role.Nurse,
            };

            var result = await _controller.Create(dto1);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<StaffDto>(createdAtActionResult.Value);
            Assert.Equal(201, createdAtActionResult.StatusCode);

            Assert.Equal(createdStaff.FirstName, returnValue.FirstName);
            Assert.Equal(createdStaff.LastName, returnValue.LastName);
            Assert.Equal(createdStaff.FullName, returnValue.FullName);
            Assert.Equal(createdStaff.Phone, returnValue.Phone);
            Assert.Equal(createdStaff.LicenseNumber, returnValue.LicenseNumber);
            Assert.Equal(createdStaff.Specialization, returnValue.Specialization);
            Assert.Equal(createdStaff.Role, returnValue.Role);
        }

        [Fact]
        public async Task InvalidPhoneCreateStaff()
        {
            List<string> AvailabilitySlots = new List<string>();
            AvailabilitySlots.Add("2024 - 10 - 10T12: 00:00 / 2024 - 10 - 11T15: 00:00");
            AvailabilitySlots.Add("2024 - 10 - 14T12: 00:00 / 2024 - 10 - 19T15: 00:00");

            CreateStaffDto dto1 = new CreateStaffDto
            {
                FirstName = "ana",
                LastName = "costa",
                FullName = "ana costa",
                LicenseNumber = 1,
                Phone = "999999",
                Specialization = "spec",
                AvailabilitySlots = AvailabilitySlots,
                Role = Role.Nurse,
                RecruitmentYear = 2024
            };

            var result = await _controller.Create(dto1);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);

            Assert.Equal(400, badRequestResult.StatusCode);

            var errorMessage = badRequestResult.Value.GetType().GetProperty("Message")?.GetValue(badRequestResult.Value, null);
            
            Assert.Equal("Error: The phone number is invalid!", errorMessage);
        }

        [Fact]
        public async Task InvalidRecYearCreateStaff()
        {
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
                RecruitmentYear = -1
            };

            var result = await _controller.Create(dto1);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);

            Assert.Equal(400, badRequestResult.StatusCode);

            var errorMessage = badRequestResult.Value.GetType().GetProperty("Message")?.GetValue(badRequestResult.Value, null);
            
            Assert.Equal("Error: The year must be bigger than zero!", errorMessage);
        }
    }
}