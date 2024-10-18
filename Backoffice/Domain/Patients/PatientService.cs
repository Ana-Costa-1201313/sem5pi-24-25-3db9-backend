

using Backoffice.Domain.Shared;
using Backoffice.Domain.Patients;
using Backoffice.Domain.Patient;
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
        //Obter todos os Patients
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

        //Obter Patient por Id
        public async Task<PatientDto> GetByIdAsync(Guid id)
        {
            var patient = await _repo.GetByIdAsync(new PatientId(id));

            if(patient == null){
                return null;
            }

            return _patientMapper.ToPatientDto(patient);
        }
        // Adicionar um novo Patient
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

        //Dar delete de um Patient
        public async Task DeleteAsync(Guid id)
        {
            var patient = await _repo.GetByIdAsync(new PatientId(id));

            if(patient == null)
                throw new BusinessRuleValidationException("Error: Patient doesn't exist !!!");
                
            _repo.Remove(patient);
            await _unitOfWork.CommitAsync();
        }

    }
}

