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

        [Fact]
        public async Task NullFirstNameCreateStaff()
        {
            List<string> AvailabilitySlots = new List<string>();
            AvailabilitySlots.Add("2024 - 10 - 10T12: 00:00 / 2024 - 10 - 11T15: 00:00");
            AvailabilitySlots.Add("2024 - 10 - 14T12: 00:00 / 2024 - 10 - 19T15: 00:00");

            CreateStaffDto dto1 = new CreateStaffDto
            {
                FirstName = null,
                LastName = "costa",
                FullName = "ana costa",
                LicenseNumber = 1,
                Phone = "999999999",
                Specialization = "spec",
                AvailabilitySlots = AvailabilitySlots,
                Role = Role.Nurse,
                RecruitmentYear = 2024
            };

            var result = await _controller.Create(dto1);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);

            Assert.Equal(400, badRequestResult.StatusCode);

            var errorMessage = badRequestResult.Value.GetType().GetProperty("Message")?.GetValue(badRequestResult.Value, null);

            Assert.Equal("Error: The staff must have a first name!", errorMessage);
        }

        [Fact]
        public async Task EmptyFirstNameCreateStaff()
        {
            List<string> AvailabilitySlots = new List<string>();
            AvailabilitySlots.Add("2024 - 10 - 10T12: 00:00 / 2024 - 10 - 11T15: 00:00");
            AvailabilitySlots.Add("2024 - 10 - 14T12: 00:00 / 2024 - 10 - 19T15: 00:00");

            CreateStaffDto dto1 = new CreateStaffDto
            {
                FirstName = "",
                LastName = "costa",
                FullName = "ana costa",
                LicenseNumber = 1,
                Phone = "999999999",
                Specialization = "spec",
                AvailabilitySlots = AvailabilitySlots,
                Role = Role.Nurse,
                RecruitmentYear = 2024
            };

            var result = await _controller.Create(dto1);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);

            Assert.Equal(400, badRequestResult.StatusCode);

            var errorMessage = badRequestResult.Value.GetType().GetProperty("Message")?.GetValue(badRequestResult.Value, null);

            Assert.Equal("Error: The staff must have a first name!", errorMessage);
        }

        [Fact]
        public async Task NullLastNameCreateStaff()
        {
            List<string> AvailabilitySlots = new List<string>();
            AvailabilitySlots.Add("2024 - 10 - 10T12: 00:00 / 2024 - 10 - 11T15: 00:00");
            AvailabilitySlots.Add("2024 - 10 - 14T12: 00:00 / 2024 - 10 - 19T15: 00:00");

            CreateStaffDto dto1 = new CreateStaffDto
            {
                FirstName = "ana",
                LastName = null,
                FullName = "ana costa",
                LicenseNumber = 1,
                Phone = "999999999",
                Specialization = "spec",
                AvailabilitySlots = AvailabilitySlots,
                Role = Role.Nurse,
                RecruitmentYear = 2024
            };

            var result = await _controller.Create(dto1);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);

            Assert.Equal(400, badRequestResult.StatusCode);

            var errorMessage = badRequestResult.Value.GetType().GetProperty("Message")?.GetValue(badRequestResult.Value, null);

            Assert.Equal("Error: The staff must have a last name!", errorMessage);
        }

        [Fact]
        public async Task EmptyLastNameCreateStaff()
        {
            List<string> AvailabilitySlots = new List<string>();
            AvailabilitySlots.Add("2024 - 10 - 10T12: 00:00 / 2024 - 10 - 11T15: 00:00");
            AvailabilitySlots.Add("2024 - 10 - 14T12: 00:00 / 2024 - 10 - 19T15: 00:00");

            CreateStaffDto dto1 = new CreateStaffDto
            {
                FirstName = "ana",
                LastName = "",
                FullName = "ana costa",
                LicenseNumber = 1,
                Phone = "999999999",
                Specialization = "spec",
                AvailabilitySlots = AvailabilitySlots,
                Role = Role.Nurse,
                RecruitmentYear = 2024
            };

            var result = await _controller.Create(dto1);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);

            Assert.Equal(400, badRequestResult.StatusCode);

            var errorMessage = badRequestResult.Value.GetType().GetProperty("Message")?.GetValue(badRequestResult.Value, null);

            Assert.Equal("Error: The staff must have a last name!", errorMessage);
        }

        [Fact]
        public async Task NullFullNameCreateStaff()
        {
            List<string> AvailabilitySlots = new List<string>();
            AvailabilitySlots.Add("2024 - 10 - 10T12: 00:00 / 2024 - 10 - 11T15: 00:00");
            AvailabilitySlots.Add("2024 - 10 - 14T12: 00:00 / 2024 - 10 - 19T15: 00:00");

            CreateStaffDto dto1 = new CreateStaffDto
            {
                FirstName = "ana",
                LastName = "costa",
                FullName = null,
                LicenseNumber = 1,
                Phone = "999999999",
                Specialization = "spec",
                AvailabilitySlots = AvailabilitySlots,
                Role = Role.Nurse,
                RecruitmentYear = 2024
            };

            var result = await _controller.Create(dto1);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);

            Assert.Equal(400, badRequestResult.StatusCode);

            var errorMessage = badRequestResult.Value.GetType().GetProperty("Message")?.GetValue(badRequestResult.Value, null);

            Assert.Equal("Error: The staff must have a full name!", errorMessage);
        }

        [Fact]
        public async Task EmptyFullNameCreateStaff()
        {
            List<string> AvailabilitySlots = new List<string>();
            AvailabilitySlots.Add("2024 - 10 - 10T12: 00:00 / 2024 - 10 - 11T15: 00:00");
            AvailabilitySlots.Add("2024 - 10 - 14T12: 00:00 / 2024 - 10 - 19T15: 00:00");

            CreateStaffDto dto1 = new CreateStaffDto
            {
                FirstName = "ana",
                LastName = "costa",
                FullName = "",
                LicenseNumber = 1,
                Phone = "999999999",
                Specialization = "spec",
                AvailabilitySlots = AvailabilitySlots,
                Role = Role.Nurse,
                RecruitmentYear = 2024
            };

            var result = await _controller.Create(dto1);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);

            Assert.Equal(400, badRequestResult.StatusCode);

            var errorMessage = badRequestResult.Value.GetType().GetProperty("Message")?.GetValue(badRequestResult.Value, null);

            Assert.Equal("Error: The staff must have a full name!", errorMessage);
        }

        [Fact]
        public async Task InvalidRoleCreateStaff()
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
                Role = Role.Patient,
                RecruitmentYear = 2024
            };

            var result = await _controller.Create(dto1);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);

            Assert.Equal(400, badRequestResult.StatusCode);

            var errorMessage = badRequestResult.Value.GetType().GetProperty("Message")?.GetValue(badRequestResult.Value, null);

            Assert.Equal("Error: The staff role must be one of the following: Admin, Doctor, Nurse or Tech!", errorMessage);
        }

        [Fact]
        public async Task DeleteStaff()
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

            var result = await _controller.Deactivate(staff.Id.AsGuid());

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<StaffDto>(okResult.Value);

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("Deactivated Staff", returnValue.FirstName);
            Assert.Equal("Deactivated Staff", returnValue.LastName);
            Assert.Equal("Deactivated Staff", returnValue.FullName);
            Assert.Null(returnValue.Phone);
            Assert.Equal("Deactivated Staff", returnValue.Specialization);
            Assert.False(returnValue.Active);
        }

        [Fact]
        public async Task InvalidDeleteStaff()
        {
            _repo.Setup(repo => repo.GetByIdAsync(It.IsAny<StaffId>())).ReturnsAsync((Staff)null);

            var staffId = Guid.NewGuid();

            var result = await _controller.Deactivate(staffId);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task DeleteInactiveStaff()
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

            await _controller.Deactivate(staff.Id.AsGuid());

            var result2 = await _controller.Deactivate(staff.Id.AsGuid());

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result2.Result);

            Assert.Equal(400, badRequestResult.StatusCode);

            var errorMessage = badRequestResult.Value.GetType().GetProperty("Message")?.GetValue(badRequestResult.Value, null);

            Assert.Equal("Error: This Staff profile is already deactivated!", errorMessage);
        }

        [Fact]
        public async Task UpdateStaff()
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

            List<string> AvailabilitySlots2 = new List<string>();
            AvailabilitySlots2.Add("2024 - 10 - 10T12: 00:00 / 2024 - 10 - 11T15: 00:00");
            AvailabilitySlots2.Add("2024 - 10 - 14T12: 00:00 / 2024 - 10 - 19T15: 00:00");

            EditStaffDto editDto = new EditStaffDto
            {
                Id = staff.Id.AsGuid(),
                Phone = "999999991",
                Specialization = "spec2",
                AvailabilitySlots = AvailabilitySlots2
            };

            _repo.Setup(repo => repo.GetByIdAsync(It.IsAny<StaffId>())).ReturnsAsync(staff);

            var result = await _controller.Update(staff.Id.AsGuid(), editDto);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<StaffDto>(okResult.Value);

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("ana", returnValue.FirstName);
            Assert.Equal("costa", returnValue.LastName);
            Assert.Equal("ana costa", returnValue.FullName);
            Assert.Equal("999999991", returnValue.Phone);
            Assert.Equal("spec2", returnValue.Specialization);
        }

        [Fact]
        public async Task NullPhoneUpdateStaff()
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

            List<string> AvailabilitySlots2 = new List<string>();
            AvailabilitySlots2.Add("2024 - 10 - 10T12: 00:00 / 2024 - 10 - 11T15: 00:00");
            AvailabilitySlots2.Add("2024 - 10 - 14T12: 00:00 / 2024 - 10 - 19T15: 00:00");

            EditStaffDto editDto = new EditStaffDto
            {
                Id = staff.Id.AsGuid(),
                Phone = null,
                Specialization = "spec2",
                AvailabilitySlots = AvailabilitySlots2
            };

            _repo.Setup(repo => repo.GetByIdAsync(It.IsAny<StaffId>())).ReturnsAsync(staff);

            var result = await _controller.Update(staff.Id.AsGuid(), editDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);

            Assert.Equal(400, badRequestResult.StatusCode);

            var errorMessage = badRequestResult.Value.GetType().GetProperty("Message")?.GetValue(badRequestResult.Value, null);

            Assert.Equal("Error: The staff must have a phone number!", errorMessage);
        }

        [Fact]
        public async Task UpdateInactiveStaff()
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

            List<string> AvailabilitySlots2 = new List<string>();
            AvailabilitySlots2.Add("2024 - 10 - 10T12: 00:00 / 2024 - 10 - 11T15: 00:00");
            AvailabilitySlots2.Add("2024 - 10 - 14T12: 00:00 / 2024 - 10 - 19T15: 00:00");

            EditStaffDto editDto = new EditStaffDto
            {
                Id = staff.Id.AsGuid(),
                Phone = "999999999",
                Specialization = "spec2",
                AvailabilitySlots = AvailabilitySlots2
            };

            _repo.Setup(repo => repo.GetByIdAsync(It.IsAny<StaffId>())).ReturnsAsync(staff);

            await _controller.Deactivate(staff.Id.AsGuid());

            var result = await _controller.Update(staff.Id.AsGuid(), editDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);

            Assert.Equal(400, badRequestResult.StatusCode);

            var errorMessage = badRequestResult.Value.GetType().GetProperty("Message")?.GetValue(badRequestResult.Value, null);

            Assert.Equal("Error: Can't update an inactive staff!", errorMessage);
        }

        [Fact]
        public async Task UpdateStaffEmpty()
        {
            List<string> AvailabilitySlots2 = new List<string>();
            AvailabilitySlots2.Add("2024 - 10 - 10T12: 00:00 / 2024 - 10 - 11T15: 00:00");
            AvailabilitySlots2.Add("2024 - 10 - 14T12: 00:00 / 2024 - 10 - 19T15: 00:00");

            var staffId = new Guid("846b1792-6a40-4d97-ba61-300c934dddd3");

            EditStaffDto editDto = new EditStaffDto
            {
                Id = staffId,
                Phone = "999999999",
                Specialization = "spec2",
                AvailabilitySlots = AvailabilitySlots2
            };

            _repo.Setup(repo => repo.GetByIdAsync(It.IsAny<StaffId>())).ReturnsAsync((Staff)null);

            var result = await _controller.Update(staffId, editDto);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PartialUpdateStaff()
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

            List<string> AvailabilitySlots2 = new List<string>();
            AvailabilitySlots2.Add("2024 - 10 - 10T12: 00:00 / 2024 - 10 - 11T15: 00:00");
            AvailabilitySlots2.Add("2024 - 10 - 14T12: 00:00 / 2024 - 10 - 19T15: 00:00");

            EditStaffDto editDto = new EditStaffDto
            {
                Id = staff.Id.AsGuid(),
                Phone = "999999991",
                Specialization = "spec2",
                AvailabilitySlots = AvailabilitySlots2
            };

            _repo.Setup(repo => repo.GetByIdAsync(It.IsAny<StaffId>())).ReturnsAsync(staff);

            var result = await _controller.PartialUpdate(staff.Id.AsGuid(), editDto);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<StaffDto>(okResult.Value);

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("ana", returnValue.FirstName);
            Assert.Equal("costa", returnValue.LastName);
            Assert.Equal("ana costa", returnValue.FullName);
            Assert.Equal("999999991", returnValue.Phone);
            Assert.Equal("spec2", returnValue.Specialization);
        }

        [Fact]
        public async Task PartialUpdateStaffSamePhone()
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

            List<string> AvailabilitySlots2 = new List<string>();
            AvailabilitySlots2.Add("2024 - 10 - 10T12: 00:00 / 2024 - 10 - 11T15: 00:00");
            AvailabilitySlots2.Add("2024 - 10 - 14T12: 00:00 / 2024 - 10 - 19T15: 00:00");

            EditStaffDto editDto = new EditStaffDto
            {
                Id = staff.Id.AsGuid(),
                Phone = null,
                Specialization = "spec2",
                AvailabilitySlots = AvailabilitySlots2
            };

            _repo.Setup(repo => repo.GetByIdAsync(It.IsAny<StaffId>())).ReturnsAsync(staff);

            var result = await _controller.PartialUpdate(staff.Id.AsGuid(), editDto);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<StaffDto>(okResult.Value);

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("ana", returnValue.FirstName);
            Assert.Equal("costa", returnValue.LastName);
            Assert.Equal("ana costa", returnValue.FullName);
            Assert.Equal("999999999", returnValue.Phone);
            Assert.Equal("spec2", returnValue.Specialization);
        }

        [Fact]
        public async Task PartialUpdateStaffSameSpec()
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

            List<string> AvailabilitySlots2 = new List<string>();
            AvailabilitySlots2.Add("2024 - 10 - 10T12: 00:00 / 2024 - 10 - 11T15: 00:00");
            AvailabilitySlots2.Add("2024 - 10 - 14T12: 00:00 / 2024 - 10 - 19T15: 00:00");

            EditStaffDto editDto = new EditStaffDto
            {
                Id = staff.Id.AsGuid(),
                Phone = "999999999",
                Specialization = null,
                AvailabilitySlots = AvailabilitySlots2
            };

            _repo.Setup(repo => repo.GetByIdAsync(It.IsAny<StaffId>())).ReturnsAsync(staff);

            var result = await _controller.PartialUpdate(staff.Id.AsGuid(), editDto);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<StaffDto>(okResult.Value);

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("ana", returnValue.FirstName);
            Assert.Equal("costa", returnValue.LastName);
            Assert.Equal("ana costa", returnValue.FullName);
            Assert.Equal("999999999", returnValue.Phone);
            Assert.Equal("spec", returnValue.Specialization);
        }
    
     [Fact]
        public async Task PartialUpdateInactiveStaff()
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

            List<string> AvailabilitySlots2 = new List<string>();
            AvailabilitySlots2.Add("2024 - 10 - 10T12: 00:00 / 2024 - 10 - 11T15: 00:00");
            AvailabilitySlots2.Add("2024 - 10 - 14T12: 00:00 / 2024 - 10 - 19T15: 00:00");

            EditStaffDto editDto = new EditStaffDto
            {
                Id = staff.Id.AsGuid(),
                Phone = "999999999",
                Specialization = "spec2",
                AvailabilitySlots = AvailabilitySlots2
            };

            _repo.Setup(repo => repo.GetByIdAsync(It.IsAny<StaffId>())).ReturnsAsync(staff);

            await _controller.Deactivate(staff.Id.AsGuid());

            var result = await _controller.PartialUpdate(staff.Id.AsGuid(), editDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);

            Assert.Equal(400, badRequestResult.StatusCode);

            var errorMessage = badRequestResult.Value.GetType().GetProperty("Message")?.GetValue(badRequestResult.Value, null);

            Assert.Equal("Error: Can't update an inactive staff!", errorMessage);
        }
    }
}