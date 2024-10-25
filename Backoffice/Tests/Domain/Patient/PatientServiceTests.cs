using Backoffice.Domain.Logs;
using Backoffice.Domain.Patients;
using Backoffice.Domain.Shared;
using Backoffice.Domain.Users;
using Moq;
using Xunit;

namespace Backoffice.Tests
{
    public class PatientServiceTests
    {
        Mock<IPatientRepository> patientRepository;
        Mock<IUnitOfWork> unitOfWork;
        Mock<ILogRepository> logRepository;
        Mock<PatientService> mockService;
        Mock<IEmailService> emailService;

        private void Setup(List<Patient> patientsDataBase){
            patientRepository = new Mock<IPatientRepository>();
            unitOfWork = new Mock<IUnitOfWork>();
            logRepository = new Mock<ILogRepository>();
            emailService = new Mock<IEmailService>();

            //Mock obter todos os patients
            patientRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(patientsDataBase);

            //Mock obter patient por id 
            patientRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<PatientId>()))
            .ReturnsAsync((PatientId id) => patientsDataBase.SingleOrDefault(p=> p.Id.Equals(id)));

            //Mock para adicionar um patient
            patientRepository.Setup(repo => repo.AddAsync(It.IsAny<Patient>()))
            .Callback<Patient>(p => patientsDataBase.Add(p));
            
            //Mock para obter o ultimo patient pelo mês
            patientRepository.Setup(repo => repo.GetLatestPatientByMonthAsync())
            .ReturnsAsync(()=> patientsDataBase.OrderByDescending(p => p.MedicalRecordNumber).FirstOrDefault());

            //Mock para remover o patient
            patientRepository.Setup(repo => repo.Remove(It.IsAny<Patient>()))
            .Callback<Patient>(p => patientsDataBase.Remove(p));

            //Mock para o searchPatients
            patientRepository.Setup(repo => repo.SearchPatientsAsync(It.IsAny<string>(),It.IsAny<string>(),It.IsAny<DateTime?>(),It.IsAny<string>()))
            .ReturnsAsync((string name,string email,DateTime? dateOfBirth,string medicalRecordNumber) =>
            patientsDataBase.Where(p=>
            (string.IsNullOrEmpty(name) || p.FullName.Contains(name) ) &&
            (string.IsNullOrEmpty(email) || p.Email._Email == email) &&
            (!dateOfBirth.HasValue || p.DateOfBirth.Date == dateOfBirth.Value.Date) &&
            (string.IsNullOrEmpty(medicalRecordNumber) || p.MedicalRecordNumber.Contains(medicalRecordNumber))
        ).ToList());

            logRepository.Setup(repo => repo.AddAsync(It.IsAny<Log>()))
            .ReturnsAsync(new Mock<Log>().Object);

            unitOfWork.Setup(uow => uow.CommitAsync()).ReturnsAsync(0);
            
            mockService = new Mock<PatientService>(unitOfWork.Object,patientRepository.Object,new PatientMapper(),logRepository.Object,emailService.Object);
        }
       [Fact]
    public async Task GetAllAsync()
    {
            var patientsDataBase = new List<Patient>();

            Setup(patientsDataBase);
            mockService.CallBase = true;
            
            PatientService service = mockService.Object;

            var patientDto1 = new CreatePatientDto
            {
                 FirstName = "John",
                LastName = "Stones",
                FullName = "John Stones",
                Gender = "M",
                DateOfBirth = new DateTime(1994,5,28),
                Email = "johnStones@gmail.com",
                Phone = "999888777",
                EmergencyContact = "939111222"
            };
            var patientDto2 = new CreatePatientDto
            {
                 FirstName = "Kyle",
                LastName = "Walker",
                FullName = "Kyle Walker",
                Gender = "M",
                DateOfBirth = new DateTime(1990,5,28),
                Email = "kyleWalker@gmail.com",
                Phone = "999888213",
                EmergencyContact = "919111222"
            };

            var patient1 = new Patient(patientDto1,"202310000001");
            var patient2 = new Patient(patientDto2,"202310000002");

            patientsDataBase.Add(patient1);
            patientsDataBase.Add(patient2);

            var result = await service.GetAllAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            Assert.Equal("John Stones",result[0].FullName);
            Assert.Equal("M",result[0].Gender);
            Assert.Equal(new DateTime(1994,5,28),result[0].DateOfBirth);
            Assert.Equal("johnStones@gmail.com",result[0].Email);
            Assert.Equal("999888777",result[0].Phone);
            Assert.Equal("202310000001",result[0].MedicalRecordNumber);

            Assert.Equal("Kyle Walker",result[1].FullName);
            Assert.Equal("M",result[1].Gender);
            Assert.Equal(new DateTime(1990,5,28),result[1].DateOfBirth);
            Assert.Equal("kyleWalker@gmail.com",result[1].Email);
            Assert.Equal("999888213",result[1].Phone);
            Assert.Equal("202310000002",result[1].MedicalRecordNumber);
           
    }

    [Fact]

    public async Task GetByIdAsync()
    {
         var patientsDataBase = new List<Patient>();

            Setup(patientsDataBase);
            mockService.CallBase = true;
            
            PatientService service = mockService.Object;

            var patientDto1 = new CreatePatientDto
            {
                 FirstName = "John",
                LastName = "Stones",
                FullName = "John Stones",
                Gender = "M",
                DateOfBirth = new DateTime(1994,5,28),
                Email = "johnStones@gmail.com",
                Phone = "999888777",
                EmergencyContact = "939111222"
            };
            var patientDto2 = new CreatePatientDto
            {
                 FirstName = "Kyle",
                LastName = "Walker",
                FullName = "Kyle Walker",
                Gender = "M",
                DateOfBirth = new DateTime(1990,5,28),
                Email = "kyleWalker@gmail.com",
                Phone = "999888213",
                EmergencyContact = "919111222"
            };

            var patient1 = new Patient(patientDto1,"202310000001");
            var patient2 = new Patient(patientDto2,"202310000002");

            patientsDataBase.Add(patient1);
            patientsDataBase.Add(patient2);

            var result = await service.GetByIdAsync(patient1.Id.AsGuid());

            Assert.NotNull(result);
            Assert.Equal("John Stones",result.FullName);
            Assert.Equal("M",result.Gender);
            Assert.Equal(new DateTime(1994,5,28),result.DateOfBirth);
            Assert.Equal("johnStones@gmail.com",result.Email);
            Assert.Equal("999888777",result.Phone);
            Assert.Equal("202310000001",result.MedicalRecordNumber);

    }
    [Fact]
    public async Task GetByIdThatDoesNotExist()
    {
        var patientsDataBase = new List<Patient>();

            Setup(patientsDataBase);
            mockService.CallBase = true;
            
            PatientService service = mockService.Object;

            var patientDto1 = new CreatePatientDto
            {
                 FirstName = "John",
                LastName = "Stones",
                FullName = "John Stones",
                Gender = "M",
                DateOfBirth = new DateTime(1994,5,28),
                Email = "johnStones@gmail.com",
                Phone = "999888777",
                EmergencyContact = "939111222"
            };
            var patientDto2 = new CreatePatientDto
            {
                 FirstName = "Kyle",
                LastName = "Walker",
                FullName = "Kyle Walker",
                Gender = "M",
                DateOfBirth = new DateTime(1990,5,28),
                Email = "kyleWalker@gmail.com",
                Phone = "999888213",
                EmergencyContact = "919111222"
            };

            var patient1 = new Patient(patientDto1,"202310000001");
            var patient2 = new Patient(patientDto2,"202310000002");

            patientsDataBase.Add(patient1);

            var result = await service.GetByIdAsync(patient2.Id.AsGuid());
            Assert.Null(result);
    }

    
    }

    
}