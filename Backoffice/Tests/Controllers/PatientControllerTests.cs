using Backoffice.Controllers;
using Backoffice.Domain.Logs;
using Backoffice.Domain.Patient;
using Backoffice.Domain.Patients;
using Backoffice.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Backoffice.Tests
{
    public class PatientControllerTests
    {
        Mock<IPatientRepository> _repo;
        Mock<IUnitOfWork> _unitOfWork;
        Mock<ILogRepository> _logRepo;
        Mock<IExternalApiServices> _mockExternal;
        Mock<AuthService> _mockAuthService;
        PatientService patientService;
        PatientController patientController;
        Mock<IEmailService> emailService;

        private void Setup(){
            _repo = new Mock<IPatientRepository>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _logRepo = new Mock<ILogRepository>();

            _mockExternal = new Mock<IExternalApiServices>();
            _mockAuthService = new Mock<AuthService>(_mockExternal.Object);

            emailService = new Mock<IEmailService>();
            patientService = new PatientService(_unitOfWork.Object, _repo.Object,new PatientMapper(),_logRepo.Object,emailService.Object);

            patientController = new PatientController(patientService, _mockAuthService.Object);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "Bearer someToken";
            patientController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }


       [Fact]
        public async Task GetAllPatients(){

                Setup();

                _mockAuthService.Setup(auth => auth.IsAuthorized(It.IsAny<HttpRequest>(), It.IsAny<List<string>>()))
                           .ReturnsAsync(true);

                
                

                var patientDto1 = new CreatePatientDto
            {
                 FirstName = "Jeremy",
                LastName = "Doku",
                FullName = "Jeremy Doke",
                Gender = "M",
                DateOfBirth = new DateTime(2002,5,27),
                Email = "jeremyDoku@gmail.com",
                Phone = "929888777",
                EmergencyContact = "929111222"
            };
            var patientDto2 = new CreatePatientDto
            {
                 FirstName = "Savio",
                LastName = "Moreira",
                FullName = "Savio Moreira",
                Gender = "M",
                DateOfBirth = new DateTime(2004,4,10),
                Email = "savinho@gmail.com",
                Phone = "999888203",
                EmergencyContact = "919111202"
            };

            var patient1 = new Patient(patientDto1,"202310000001");
            var patient2 = new Patient(patientDto2,"202310000002");

            var list = new List<Patient> {patient1,patient2};

            _repo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(list);

            var result = await patientController.GetAll();

            
            var objResult = Assert.IsType<OkObjectResult>(result.Result);
            var listFinal = Assert.IsAssignableFrom<List<PatientDto>>(objResult.Value);

            Assert.Equal(list.Count,listFinal.Count);
            
        }

        [Fact]
        public async Task SearchPatientsByName()
        {
                Setup();
                _mockAuthService.Setup(auth => auth.IsAuthorized(It.IsAny<HttpRequest>(), It.IsAny<List<string>>()))
                           .ReturnsAsync(true);

                            var patientDto1 = new CreatePatientDto  
                {
                    FirstName = "Joao",
                    LastName = "Mario",
                    FullName = "Joao Mario",
                    Gender = "M",
                    DateOfBirth = new DateTime(1993,1,19),
                    Email = "joaoMarioBenfica@gmail.com",
                    Phone = "919800011",
                    EmergencyContact = "934111011"
                };
            var patientDto2 = new CreatePatientDto  
                {
                    FirstName = "Joao",
                    LastName = "Mario",
                    FullName = "Joao Mario",
                    Gender = "M",
                    DateOfBirth = new DateTime(2000,1,3),
                    Email = "joaoMarioPorto@gmail.com",
                    Phone = "919800012",
                    EmergencyContact = "934111012"
                };
            var patientDto3 = new CreatePatientDto  
                {
                    FirstName = "Viktor",
                    LastName = "Gyokeres",
                    FullName = "Viktor Gyokeres",
                    Gender = "M",
                    DateOfBirth = new DateTime(1998,6,4),
                    Email = "gyokeresSporting@gmail.com",
                    Phone = "919800013",
                    EmergencyContact = "934111013"
                };

                var patient1 = new Patient(patientDto1,"199901000001");
                var patient2 = new Patient(patientDto2,"199901000002");
                var patient3 = new Patient(patientDto3,"199901000003");

                var list = new List<Patient> {patient1, patient2, patient3};
                _repo.Setup(repo => repo.SearchPatientsAsync("Joao",null,null,null)).ReturnsAsync(list.Where(p => p.FirstName == "Joao").ToList());

                var result = await patientController.SearchPatients("Joao",null,null,null);

                var objResult = Assert.IsType<OkObjectResult>(result.Result);
                var resultList = Assert.IsType<List<SearchPatientDto>>(objResult.Value);

                Assert.Equal(2,resultList.Count);
                Assert.Equal("Joao Mario",resultList[0].FullName);
                Assert.Equal("Joao Mario",resultList[1].FullName);
                Assert.NotEqual("Viktor Gyokeres",resultList[0].FullName);
                Assert.NotEqual("Viktor Gyokeres",resultList[1].FullName);
        }
    }
}