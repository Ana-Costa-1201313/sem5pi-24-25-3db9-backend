using Backoffice.Controllers;
using Backoffice.Domain.Shared;
using Backoffice.Domain.Staffs;
using Backoffice.Domain.Specializations;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Backoffice.Domain.Logs;


namespace Backoffice.Tests
{
    public class StaffControllerTest
    {
        Mock<IStaffRepository> _repo;
        Mock<ISpecializationRepository> _specRepo;
        Mock<IUnitOfWork> _unitOfWork;
        Mock<ILogRepository> _logRepo;
        Mock<IExternalApiServices> _mockExternal;
        Mock<AuthService> _mockAuthService;
        Mock<StaffService> mockService;
        Mock<StaffController> mockController;


        private void Setup()
        {
            _repo = new Mock<IStaffRepository>();
            _specRepo = new Mock<ISpecializationRepository>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _logRepo = new Mock<ILogRepository>();
            _mockExternal = new Mock<IExternalApiServices>();
            _mockAuthService = new Mock<AuthService>(_mockExternal.Object);
            mockService = new Mock<StaffService>(_unitOfWork.Object, _repo.Object, new StaffMapper(), _specRepo.Object, _logRepo.Object);

            mockController = new Mock<StaffController>(mockService.Object, _mockAuthService.Object);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "Bearer someToken";

            mockController.Object.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }

        [Fact]
        public async Task GetAllStaff()
        {
            Setup();

            _mockAuthService.Setup(auth => auth.IsAuthorized(It.IsAny<HttpRequest>(), It.IsAny<List<string>>()))
                           .ReturnsAsync(true);

            _specRepo.Setup(specRepo => specRepo.GetBySpecializationName(It.IsAny<string>()))
                        .ReturnsAsync(new Specialization(new SpecializationName("skin")));


            mockService.CallBase = true;
            mockController.CallBase = true;

            mockService.Setup(servico => servico.ReadDNS()).Returns("myhealthcareapp.com");

            StaffController controller = mockController.Object;

            List<string> AvailabilitySlots = new List<string>();
            AvailabilitySlots.Add("2024 - 10 - 10T12: 00:00 / 2024 - 10 - 11T15: 00:00");
            AvailabilitySlots.Add("2024 - 10 - 14T12: 00:00 / 2024 - 10 - 19T15: 00:00");

            CreateStaffDto dto1 = new CreateStaffDto
            {
                Name = "ana costa",
                LicenseNumber = 1,
                Phone = "999999999",
                Specialization = "skin",
                AvailabilitySlots = AvailabilitySlots,
                Role = Role.Nurse,
                RecruitmentYear = 2024
            };

            CreateStaffDto dto2 = new CreateStaffDto
            {
                Name = "maria silva",
                LicenseNumber = 2,
                Phone = "999999989",
                Specialization = "skin",
                AvailabilitySlots = AvailabilitySlots,
                Role = Role.Doctor,
                RecruitmentYear = 2024
            };

            Specialization spec = new Specialization(new SpecializationName("skin"));

            var staff1 = new Staff(dto1, spec, 1, "hotmail.com");
            var staff2 = new Staff(dto2, spec, 2, "hotmail.com");

            var expectedList = new List<Staff> { staff1, staff2 };

            _repo.Setup(repo => repo.GetAllWithDetailsAsync()).ReturnsAsync(expectedList);

            var result = await mockController.Object.GetAll();

            var okResult = Assert.IsType<ActionResult<IEnumerable<StaffDto>>>(result);

            var objectResult = Assert.IsType<OkObjectResult>(okResult.Result);

            var actualList = Assert.IsAssignableFrom<IEnumerable<StaffDto>>(objectResult.Value);

            Assert.Equal(expectedList.Count, actualList.Count());
        }

        [Fact]
        public async Task GetAllEmpty()
        {
            Setup();

            _mockAuthService.Setup(auth => auth.IsAuthorized(It.IsAny<HttpRequest>(), It.IsAny<List<string>>()))
                           .ReturnsAsync(true);

            _specRepo.Setup(specRepo => specRepo.GetBySpecializationName(It.IsAny<string>()))
                        .ReturnsAsync(new Specialization(new SpecializationName("skin")));


            mockService.CallBase = true;
            mockController.CallBase = true;

            mockService.Setup(servico => servico.ReadDNS()).Returns("myhealthcareapp.com");

            StaffController controller = mockController.Object;

            var expectedList = new List<Staff>();

            _repo.Setup(repo => repo.GetAllWithDetailsAsync())
            .ReturnsAsync(expectedList);

            var result = await mockController.Object.GetAll();

            var noContentResult = Assert.IsType<ActionResult<IEnumerable<StaffDto>>>(result);

            Assert.IsType<NoContentResult>(noContentResult.Result);
        }

        [Fact]
        public async Task GetByIdStaff()
        {
           Setup();

            _mockAuthService.Setup(auth => auth.IsAuthorized(It.IsAny<HttpRequest>(), It.IsAny<List<string>>()))
                           .ReturnsAsync(true);

            _specRepo.Setup(specRepo => specRepo.GetBySpecializationName(It.IsAny<string>()))
                        .ReturnsAsync(new Specialization(new SpecializationName("skin")));


            mockService.CallBase = true;
            mockController.CallBase = true;

            mockService.Setup(servico => servico.ReadDNS()).Returns("myhealthcareapp.com");

            StaffController controller = mockController.Object;

            List<string> AvailabilitySlots = new List<string>();
            AvailabilitySlots.Add("2024 - 10 - 10T12: 00:00 / 2024 - 10 - 11T15: 00:00");
            AvailabilitySlots.Add("2024 - 10 - 14T12: 00:00 / 2024 - 10 - 19T15: 00:00");

            CreateStaffDto dto1 = new CreateStaffDto
            {
                Name = "ana costa",
                LicenseNumber = 1,
                Phone = "999999999",
                Specialization = "skin",
                AvailabilitySlots = AvailabilitySlots,
                Role = Role.Nurse,
                RecruitmentYear = 2024
            };

            Specialization spec = new Specialization(new SpecializationName("skin"));

            var staff = new Staff(dto1, spec, 1, "hotmail.com");

            _repo.Setup(repo => repo.GetByIdWithDetailsAsync(It.IsAny<StaffId>())).ReturnsAsync(staff);

            var result = await mockController.Object.GetById(staff.Id.AsGuid());

            var okResult = Assert.IsType<ActionResult<StaffDto>>(result);

            var objectResult = Assert.IsType<OkObjectResult>(okResult.Result);

            var actualStaff = Assert.IsType<StaffDto>(objectResult.Value);

            Assert.Equal(staff.Name, actualStaff.Name);
            Assert.Equal(staff.LicenseNumber, actualStaff.LicenseNumber);
            Assert.Equal(staff.Specialization.Name.Name, actualStaff.Specialization);
            Assert.Equal(staff.Role, actualStaff.Role);
        }

        [Fact]
        public async Task GetByIdEmpty()
        {
          Setup();

            _mockAuthService.Setup(auth => auth.IsAuthorized(It.IsAny<HttpRequest>(), It.IsAny<List<string>>()))
                           .ReturnsAsync(true);

            _specRepo.Setup(specRepo => specRepo.GetBySpecializationName(It.IsAny<string>()))
                        .ReturnsAsync(new Specialization(new SpecializationName("skin")));


            mockService.CallBase = true;
            mockController.CallBase = true;

            mockService.Setup(servico => servico.ReadDNS()).Returns("myhealthcareapp.com");

            StaffController controller = mockController.Object;

            var staffId = Guid.NewGuid();

            _repo.Setup(repo => repo.GetByIdWithDetailsAsync(It.IsAny<StaffId>())).ReturnsAsync((Staff)null);

            var result = await mockController.Object.GetById(staffId);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateStaff()
        {
            Setup();

            _mockAuthService.Setup(auth => auth.IsAuthorized(It.IsAny<HttpRequest>(), It.IsAny<List<string>>()))
                           .ReturnsAsync(true);

            _specRepo.Setup(specRepo => specRepo.GetBySpecializationName(It.IsAny<string>()))
                        .ReturnsAsync(new Specialization(new SpecializationName("skin")));


            mockService.CallBase = true;
            mockController.CallBase = true;

            mockService.Setup(servico => servico.ReadDNS()).Returns("myhealthcareapp.com");

            StaffController controller = mockController.Object;

            List<string> AvailabilitySlots = new List<string>();
            AvailabilitySlots.Add("2024 - 10 - 10T12: 00:00 / 2024 - 10 - 11T15: 00:00");
            AvailabilitySlots.Add("2024 - 10 - 14T12: 00:00 / 2024 - 10 - 19T15: 00:00");

            CreateStaffDto dto1 = new CreateStaffDto
            {
                Name = "ana costa",
                LicenseNumber = 1,
                Phone = "999999999",
                Specialization = "skin",
                AvailabilitySlots = AvailabilitySlots,
                Role = Role.Nurse,
                RecruitmentYear = 2024
            };

            StaffDto createdStaff = new StaffDto
            {
                Id = Guid.NewGuid(),
                Name = "ana costa",
                LicenseNumber = 1,
                Phone = "999999999",
                Specialization = "skin",
                AvailabilitySlots = AvailabilitySlots,
                Role = Role.Nurse,
            };

            var result = await mockController.Object.Create(dto1);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<StaffDto>(createdAtActionResult.Value);

            Assert.Equal(201, createdAtActionResult.StatusCode);

            Assert.Equal(createdStaff.Name, returnValue.Name);
            Assert.Equal(createdStaff.Phone, returnValue.Phone);
            Assert.Equal(createdStaff.LicenseNumber, returnValue.LicenseNumber);
            Assert.Equal(createdStaff.Specialization, returnValue.Specialization);
            Assert.Equal(createdStaff.Role, returnValue.Role);
        }

        [Fact]
        public async Task InvalidPhoneCreateStaff()
        {
            Setup();

            _mockAuthService.Setup(auth => auth.IsAuthorized(It.IsAny<HttpRequest>(), It.IsAny<List<string>>()))
                           .ReturnsAsync(true);

            _specRepo.Setup(specRepo => specRepo.GetBySpecializationName(It.IsAny<string>()))
                        .ReturnsAsync(new Specialization(new SpecializationName("skin")));


            mockService.CallBase = true;
            mockController.CallBase = true;

            mockService.Setup(servico => servico.ReadDNS()).Returns("myhealthcareapp.com");

            StaffController controller = mockController.Object;

            List<string> AvailabilitySlots = new List<string>();
            AvailabilitySlots.Add("2024 - 10 - 10T12: 00:00 / 2024 - 10 - 11T15: 00:00");
            AvailabilitySlots.Add("2024 - 10 - 14T12: 00:00 / 2024 - 10 - 19T15: 00:00");

            CreateStaffDto dto1 = new CreateStaffDto
            {
                Name = "ana costa",
                LicenseNumber = 1,
                Phone = "999999",
                Specialization = "skin",
                AvailabilitySlots = AvailabilitySlots,
                Role = Role.Nurse,
                RecruitmentYear = 2024
            };

            var result = await mockController.Object.Create(dto1);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);

            Assert.Equal(400, badRequestResult.StatusCode);

            var errorMessage = badRequestResult.Value.GetType().GetProperty("Message")?.GetValue(badRequestResult.Value, null);

            Assert.Equal("Error: The phone number is invalid!", errorMessage);
        }

        [Fact]
        public async Task InvalidRecYearCreateStaff()
        {
            Setup();

            _mockAuthService.Setup(auth => auth.IsAuthorized(It.IsAny<HttpRequest>(), It.IsAny<List<string>>()))
                           .ReturnsAsync(true);

            _specRepo.Setup(specRepo => specRepo.GetBySpecializationName(It.IsAny<string>()))
                        .ReturnsAsync(new Specialization(new SpecializationName("skin")));


            mockService.CallBase = true;
            mockController.CallBase = true;

            mockService.Setup(servico => servico.ReadDNS()).Returns("myhealthcareapp.com");

            StaffController controller = mockController.Object;


            List<string> AvailabilitySlots = new List<string>();
            AvailabilitySlots.Add("2024 - 10 - 10T12: 00:00 / 2024 - 10 - 11T15: 00:00");
            AvailabilitySlots.Add("2024 - 10 - 14T12: 00:00 / 2024 - 10 - 19T15: 00:00");

            CreateStaffDto dto1 = new CreateStaffDto
            {
                Name = "ana costa",
                LicenseNumber = 1,
                Phone = "999999999",
                Specialization = "skin",
                AvailabilitySlots = AvailabilitySlots,
                Role = Role.Nurse,
                RecruitmentYear = -1
            };

            var result = await mockController.Object.Create(dto1);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);

            Assert.Equal(400, badRequestResult.StatusCode);

            var errorMessage = badRequestResult.Value.GetType().GetProperty("Message")?.GetValue(badRequestResult.Value, null);

            Assert.Equal("Error: The year must be bigger than zero!", errorMessage);
        }
    }
}