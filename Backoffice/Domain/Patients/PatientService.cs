

using Backoffice.Domain.Shared;
using Backoffice.Domain.Patients;
using Microsoft.EntityFrameworkCore;


namespace Backoffice.Domain.Patients{


    public class PatientService{

        private readonly IUnitOfWork _unitOfWork;
        private readonly IPatientRepository _repo;
        private readonly PatientMapper _patientMapper;


        public PatientService(IUnitOfWork unitOfWork, IPatientRepository patientRepository, PatientMapper patientMapper){
            _unitOfWork = unitOfWork;
            _repo = patientRepository;
            _patientMapper = patientMapper;
        }
        //Obter todos os Patient profiles
        public async Task<List<PatientDto>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            List<PatientDto> listDto = new List<PatientDto>();

            foreach(var patient in list )
            {
                listDto.Add(_patientMapper.ToPatientDto(patient));
            }
            return listDto;
        }
        // Obter os patient profiles por nome 
        public async Task<List<SearchPatientDto>> GetPatientsByName(string name)
         {
            var list = await _repo.GetPatientsByNameAsync(name);
            List<SearchPatientDto> listDto = new List<SearchPatientDto>();

            foreach(var patient in list)
                listDto.Add(_patientMapper.ToSearchPatientDto(patient));

                return listDto;
         }
        // Obter os patient profiles por data de nascimento
         public async Task<List<SearchPatientDto>> GetPatientsByDateOfBirth(DateTime dateOfBirth)
         {
            var list = await _repo.GetPatientsByDateOfBirth(dateOfBirth);
            List<SearchPatientDto> listDto = new List<SearchPatientDto>();

            foreach(var patient in list)
                listDto.Add(_patientMapper.ToSearchPatientDto(patient));
                
                return listDto;
         }
         // Obter os patient profiles por medical record number
         public async Task<List<SearchPatientDto>> GetPatientsByMedicalRecordNumber(int medicalRecordNumber)
         {
            var list = await _repo.GetPatientsByMedicalRecordNumber(medicalRecordNumber);
            List<SearchPatientDto> listDto = new List<SearchPatientDto>();

            foreach(var patient in list)
                listDto.Add(_patientMapper.ToSearchPatientDto(patient));
                
                return listDto;
         }
         public async Task<SearchPatientDto> GetByEmailAsync(Email email)
        {
            var patient = await _repo.GetPatientByEmailAsync(email);

            if(patient == null){
                return null;
            }

            return _patientMapper.ToSearchPatientDto(patient);
        }

        //Obter Patient profile por Id
        public async Task<PatientDto> GetByIdAsync(Guid id)
        {
            var patient = await _repo.GetByIdAsync(new PatientId(id));

            if(patient == null){
                return null;
            }

            return _patientMapper.ToPatientDto(patient);
        }
        // Adicionar um novo Patient profile
        public async Task<PatientDto> AddAsync(CreatePatientDto dto)
        {
            var patient = _patientMapper.ToPatient(dto);

            try 
            {
                await _repo.AddAsync(patient);
                await _unitOfWork.CommitAsync();
            }
            catch (DbUpdateException e)
            {
                if(e.InnerException != null && e.InnerException.Message.Contains("UNIQUE constraint failed: Patients.Email"))
                {
                    throw new BusinessRuleValidationException("Error: This email is already in use !!!");
                }
                if(e.InnerException != null && e.InnerException.Message.Contains("UNIQUE constraint failed: Patients.Phone"))
                {
                    throw new BusinessRuleValidationException("Erroe: This Phone Number is already in use !!!");
                }
                else
                {
                    throw new BusinessRuleValidationException("Error: Can't save this patient data !!!");
                }
            }
            return _patientMapper.ToPatientDto(patient);
        }
        // Dar um put de um patient profile
        public async Task<PatientDto> UpdateAsync(Guid id,EditPatientDto dto)
        {
            var patient = await _repo.GetByIdAsync(new PatientId(id));
            if(patient == null)
                return null;

            patient.UpdateDetails(dto.FirstName,dto.LastName,dto.FullName,dto.Email,dto.Phone,dto.Allergies,dto.EmergencyContact);

            await _unitOfWork.CommitAsync();
            return _patientMapper.ToPatientDto(patient);
        }
        //Dar um patch de um patient profile
        public async Task<PatientDto> PatchAsync(Guid id, EditPatientDto dto)
        {
            var patient = await _repo.GetByIdAsync(new PatientId(id));
            if(patient == null)
                return null;

           if(!string.IsNullOrEmpty(dto.FirstName))
            patient.ChangeFirstName(dto.FirstName);

            if(!string.IsNullOrEmpty(dto.LastName))
            patient.ChangeLastName(dto.LastName);

            if(!string.IsNullOrEmpty(dto.FullName))
            patient.ChangeFullName(dto.FullName);

            if(!string.IsNullOrEmpty(dto.Email))
            patient.ChangeEmail(dto.Email);

            if(!string.IsNullOrEmpty(dto.Phone))
            patient.ChangePhone(dto.Phone);

            if(dto.Allergies != null)
            patient.ChangeAllergies(dto.Allergies);

            if(!string.IsNullOrEmpty(dto.EmergencyContact))
            patient.ChangeEmergencyContact(dto.EmergencyContact);

            await _unitOfWork.CommitAsync();
            return _patientMapper.ToPatientDto(patient);
        }

        //Dar delete de um Patient profile
        public async Task<PatientDto> DeleteAsync(Guid id)
        {
            var patient = await _repo.GetByIdAsync(new PatientId(id));

            if(patient == null)
                throw new BusinessRuleValidationException("Error: Patient doesn't exist !!!");
                
            _repo.Remove(patient);
            await _unitOfWork.CommitAsync();
            return new PatientDto
            {
                Id = patient.Id.AsGuid(),
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                FullName = patient.FullName,
                Gender = patient.Gender,
                DateOfBirth = patient.DateOfBirth,
                Email = patient.Email._Email,
                Phone = patient.Phone.PhoneNum,
                Allergies = patient.Allergies,
                MedicalRecordNumber = patient.MedicalRecordNumber
            };
        }



    }
}

