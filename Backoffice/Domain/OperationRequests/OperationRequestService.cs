using Backoffice.Domain.Shared;
using Backoffice.Domain.Patients;
using Backoffice.Domain.OperationTypes;
using Backoffice.Domain.Staffs;
using Backoffice.Domain.OperationRequests.ValueObjects;
using Microsoft.IdentityModel.Tokens;

namespace Backoffice.Domain.OperationRequests
{
    public class OperationRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOperationRequestRepository _repo;
        private readonly IOperationTypeRepository _optyperepo;
        private readonly IPatientRepository _patientrepo;
        private readonly IStaffRepository _doctorrepo;

        public OperationRequestService(IUnitOfWork unitOfWork, IOperationRequestRepository repo, 
                                        IOperationTypeRepository optyperepo, IPatientRepository patientrepo, 
                                        IStaffRepository doctorrepo)
        {
            _unitOfWork = unitOfWork;
            _repo = repo;
            _optyperepo = optyperepo;
            _patientrepo = patientrepo;
            _doctorrepo = doctorrepo;
        }

        public async Task<List<OperationRequestDto>> GetAllAsync()
        {
            var list = await this._repo.GetAllAsync();

            List<OperationRequestDto> listDto = new List<OperationRequestDto>();
            
            foreach (var item in list)
            {
                listDto.Add(OperationRequestMapper.ToDto(item));
            }

            return listDto;
            //return null;
        }

        public async Task<List<OperationRequestDto>> GetAllByPatientEmailAsDoctorAsync(string doctorEmail, string patientEmail)
        {
            var list = await this._repo.GetOpRequestsByPatientEmailAsDoctorAsync(new Email(doctorEmail), new Email(patientEmail));

            List<OperationRequestDto> listDto = new List<OperationRequestDto>();
            
            foreach (var item in list)
            {
                listDto.Add(OperationRequestMapper.ToDto(item));
            }

            if (listDto.IsNullOrEmpty())
                throw new BusinessRuleValidationException("Error: No Operation Requests for the patient with the email " + patientEmail + " type found!");

            return listDto;
            //return null;
        }

        public async Task<List<OperationRequestDto>> GetAllByPriorityAsDoctorAsync(string doctorEmail, string priority)
        {
            var priorityEnum = (Priority)Enum.Parse(typeof(Priority), priority, true);
            var list = await this._repo.GetOpRequestsByPriorityAsDoctorAsync(new Email(doctorEmail), priorityEnum);

            List<OperationRequestDto> listDto = new List<OperationRequestDto>();
            
            foreach (var item in list)
            {
                listDto.Add(OperationRequestMapper.ToDto(item));
            }

            if (listDto.IsNullOrEmpty())
                throw new BusinessRuleValidationException("Error: No Operation Requests with the " + priority + " priority found!");

            return listDto;
            //return null;
        }

        public async Task<List<OperationRequestDto>> GetAllByStatusAsDoctorAsync(string doctorEmail, string status)
        {
            var statusEnum = (Status)Enum.Parse(typeof(Status), status, true);
            var list = await this._repo.GetOpRequestsByStatusAsDoctorAsync(new Email(doctorEmail), statusEnum);

            List<OperationRequestDto> listDto = new List<OperationRequestDto>();
            
            foreach (var item in list)
            {
                listDto.Add(OperationRequestMapper.ToDto(item));
            }

            if (listDto.IsNullOrEmpty())
                throw new BusinessRuleValidationException("Error: No Operation Requests with the " + status + " status found!");

            return listDto;
            //return null;
        }

        public async Task<List<OperationRequestDto>> GetAllByOpTypeNameAsDoctorAsync(string doctorEmail, string opTypeName)
        {
            var list = await this._repo.GetOpRequestsByOperationTypeNameAsDoctorAsync(new Email(doctorEmail), new OperationTypeName(opTypeName));

            List<OperationRequestDto> listDto = new List<OperationRequestDto>();
            
            foreach (var item in list)
            {
                listDto.Add(OperationRequestMapper.ToDto(item));
            }

            if (listDto.IsNullOrEmpty())
                throw new BusinessRuleValidationException("Error: No Operation Requests for the " + opTypeName + " type found!");

            return listDto;
            //return null;
        }

        public async Task<List<OperationRequestDto>> GetAllByDoctorEmailAsync(string doctorEmail)
        {
            var list = await this._repo.GetOpRequestsByDoctorEmailAsync(new Email(doctorEmail));

            List<OperationRequestDto> listDto = new List<OperationRequestDto>();
            
            foreach (var item in list)
            {
                listDto.Add(OperationRequestMapper.ToDto(item));
            }

            if (listDto.IsNullOrEmpty())
                throw new BusinessRuleValidationException("Error: No Operation Requests found!");

            return listDto;
            //return null;
        }

        public async Task<OperationRequestDto> GetByIdAsync(Guid id)
        {
            var opReq = await this._repo.GetByIdAsync(new OperationRequestId(id));

            if (opReq == null)
                throw new BusinessRuleValidationException("Error: Operation Request not found!");

            return OperationRequestMapper.ToDto(opReq);
            //return null;
        }

        public async Task<OperationRequestDto> AddAsync(CreateOperationRequestDto dto)
        {
            var opType = await this._optyperepo.GetByOperationTypeName(dto.OpTypeName);
            var patient = await this._patientrepo.GetPatientByEmailAsync(new Email(dto.PatientEmail));
            var doctor = await this._doctorrepo.GetStaffByEmailAsync(new Email(dto.DoctorEmail));

            var opRequest = OperationRequestMapper.ToDomain(dto, opType, patient, doctor);

            await this._repo.AddAsync(opRequest);

            await this._unitOfWork.CommitAsync();

            return OperationRequestMapper.ToDto(opRequest);
            //return null;
        }

        public async Task<OperationRequestDto> PatchAsync(Guid id, EditOperationRequestDto dto)
        {
            var opReq = await this._repo.GetByIdAsync(new OperationRequestId(id));

            if (opReq == null)
                throw new BusinessRuleValidationException("Error: Operation Request not found!");

            var deadlineDate = DateTime.Parse(dto.DeadlineDate);

            opReq.UpdateOperationRequest(deadlineDate, dto.Priority, dto.Description);

            await this._unitOfWork.CommitAsync();

            return OperationRequestMapper.ToDto(opReq);
            //return null;
        }

        public async Task<OperationRequestDto> DeleteAsync(Guid id)
        {
            var opReq = await this._repo.GetByIdAsync(new OperationRequestId(id));

            if (opReq == null)
                return null;

            await this._repo.DeleteOpRequestAsync(opReq.Id);

            await this._unitOfWork.CommitAsync();

            return OperationRequestMapper.ToDto(opReq);
            //return null;
        }
    }
}