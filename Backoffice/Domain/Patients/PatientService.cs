

using Backoffice.Domain.Shared;
using Backoffice.Domain.Patients;
using Backoffice.Domain.Patient;
using Microsoft.EntityFrameworkCore;
using Backoffice.Domain.Logs;
using Backoffice.Domain.Users;


namespace Backoffice.Domain.Patients{


    public class PatientService{

        private readonly IUnitOfWork _unitOfWork;
        private readonly IPatientRepository _repo;
        private readonly PatientMapper _patientMapper;
        private readonly ILogRepository _repoLog;
        private readonly EmailService _emailService;

        public PatientService(IUnitOfWork unitOfWork, IPatientRepository patientRepository, PatientMapper patientMapper, ILogRepository repoLog, EmailService emailService){
            this._unitOfWork = unitOfWork;
            this._repo = patientRepository;
            this._patientMapper = patientMapper;
            this._repoLog = repoLog;
            this._emailService = emailService; 
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
        public async Task<PatientDto> AddAsync(CreatePatientDto dto, string medicalRecordNumber)
        {
            var patient = _patientMapper.ToPatient(dto,medicalRecordNumber);
            
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
            //Guardar a informação sensível antiga
            var oldEmail = patient.Email._Email;
            var oldPhoneNumber = patient.Phone.PhoneNum;


            patient.UpdateDetails(dto.FirstName,dto.LastName,dto.FullName,dto.Email,dto.Phone,dto.Allergies,dto.EmergencyContact);
            
            //Verificar se houve mudanças
            bool emailChanged = oldEmail != dto.Email;
            bool phoneChanged = oldPhoneNumber != dto.Phone;

            if (emailChanged || phoneChanged)
            {
            var message = "Your contact information has been updated, if you didn't request this change please contact us !!!";
            var subject = "Patient Profile Updated";
        
            // Manda um email para o email antigo do patient
            await _emailService.SendEmail(oldEmail, message, subject);
            }

            await _repoLog.AddAsync(new Log(patient.ToJSON(),LogType.Update,LogEntity.Patient));

            await _unitOfWork.CommitAsync();

            return _patientMapper.ToPatientDto(patient);
        }

        //Dar um patch de um patient profile
        public async Task<PatientDto> PatchAsync(Guid id, EditPatientDto dto)
        {
            var patient = await _repo.GetByIdAsync(new PatientId(id));
            if(patient == null)
                return null;

            var oldEmail = patient.Email._Email;
            var oldPhoneNumber = patient.Phone.PhoneNum;
            
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

           
            bool emailChanged = oldEmail != dto.Email;
            bool phoneChanged = oldPhoneNumber != dto.Phone;

            if (emailChanged || phoneChanged)
            {
            var message = "Your contact information has been updated, if you didn't request this change please contact us !!!";
            var subject = "Patient Profile Updated";
        
           
            await _emailService.SendEmail(oldEmail, message, subject);
            }

            await _repoLog.AddAsync(new Log(patient.ToJSON(),LogType.Update,LogEntity.Patient));

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

            await _repoLog.AddAsync(new Log(patient.ToJSON(), LogType.Delete,LogEntity.Patient));

            await _unitOfWork.CommitAsync();
            
            return _patientMapper.ToPatientDto(patient);
        }

        //Get de patients por varios atributos 
        public async Task<List<SearchPatientDto>> SearchPatientsAsync(string name, string email, DateTime? dateOfBirth,string medicalRecordNumber)
        {
            var patients = await _repo.SearchPatientsAsync(name,email,dateOfBirth,medicalRecordNumber);
            List<SearchPatientDto> listDto = new List<SearchPatientDto>();

                foreach(var patient in patients)
                    listDto.Add(_patientMapper.ToSearchPatientDto(patient));

            return listDto;
        }
        //Gerar um medical record number de acordo com o Ano Mês e pessoal anterior 
        public async Task<string> GenerateNextMedicalRecordNumber()
        {
            DateTime date = DateTime.Now;
            int year = date.Year;
            int month = date.Month;

            var latestPatient = await _repo.GetLatestPatientByMonthAsync();
            int nextSequential = 1; // se nao existir ninguem começa com o 1

            if (latestPatient != null)
            {
                string latestMedicalRN = latestPatient.MedicalRecordNumber;
                string latestYear = latestMedicalRN.Substring(0,4);  // Vai buscar os 4 primeiros ou seja o ano
                string latestMonth = latestMedicalRN.Substring(4,2); // vai buscar os 2 seguintes ou seja o mês 
                string latestSequential = latestMedicalRN.Substring(6); //vai buscar o resto ou seja o numero sequencial

                if(int.Parse(latestYear) == year && int.Parse(latestMonth)==month)
                    nextSequential = int.Parse(latestSequential) +1;
            }
            string finalSequential = nextSequential.ToString("D6");

            string medicalRecordNumber = $"{year}{month:D2}{finalSequential}";

            return medicalRecordNumber;

        }


    }
}

