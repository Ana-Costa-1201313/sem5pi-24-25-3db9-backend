using Backoffice.Domain.Shared;
using Backoffice.Domain.Patients;
using Backoffice.Domain.OperationTypes;
using Backoffice.Domain.Staffs;
using Backoffice.Domain.OperationRequests.ValueObjects;

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

        public async Task<List<OperationRequestDto>> GetAllByPatientNameAsDoctorAsync(Guid doctorId, string patientName)
        {
            var list = await this._repo.GetOpRequestsByPatientNameAsDoctorAsync(new StaffId(doctorId), patientName);

            List<OperationRequestDto> listDto = new List<OperationRequestDto>();
            
            foreach (var item in list)
            {
                listDto.Add(OperationRequestMapper.ToDto(item));
            }

            return listDto;
            //return null;
        }

        public async Task<List<OperationRequestDto>> GetAllByPriorityAsDoctorAsync(Guid doctorId, string priority)
        {
            var priorityEnum = (Priority)Enum.Parse(typeof(Priority), priority, true);
            var list = await this._repo.GetOpRequestsByPriorityAsDoctorAsync(new StaffId(doctorId), priorityEnum);

            List<OperationRequestDto> listDto = new List<OperationRequestDto>();
            
            foreach (var item in list)
            {
                listDto.Add(OperationRequestMapper.ToDto(item));
            }

            return listDto;
            //return null;
        }

        public async Task<List<OperationRequestDto>> GetAllByStatusAsDoctorAsync(Guid doctorId, string status)
        {
            var statusEnum = (Status)Enum.Parse(typeof(Status), status, true);
            var list = await this._repo.GetOpRequestsByStatusAsDoctorAsync(new StaffId(doctorId), statusEnum);

            List<OperationRequestDto> listDto = new List<OperationRequestDto>();
            
            foreach (var item in list)
            {
                listDto.Add(OperationRequestMapper.ToDto(item));
            }

            return listDto;
            //return null;
        }

        public async Task<List<OperationRequestDto>> GetAllByOpTypeNameAsDoctorAsync(Guid doctorId, string opTypeName)
        {
            var list = await this._repo.GetOpRequestsByOperationTypeNameAsDoctorAsync(new StaffId(doctorId), new OperationTypeName(opTypeName));

            List<OperationRequestDto> listDto = new List<OperationRequestDto>();
            
            foreach (var item in list)
            {
                listDto.Add(OperationRequestMapper.ToDto(item));
            }

            return listDto;
            //return null;
        }

        public async Task<List<OperationRequestDto>> GetAllByDoctorIdAsync(Guid doctorId)
        {
            var list = await this._repo.GetOpRequestsByDoctorIdAsync(new StaffId(doctorId));

            List<OperationRequestDto> listDto = new List<OperationRequestDto>();
            
            foreach (var item in list)
            {
                listDto.Add(OperationRequestMapper.ToDto(item));
            }

            return listDto;
            //return null;
        }

        public async Task<OperationRequestDto> GetByIdAsync(Guid id)
        {
            var opReq = await this._repo.GetByIdAsync(new OperationRequestId(id));

            if (opReq == null)
                return null;

            return OperationRequestMapper.ToDto(opReq);
            //return null;
        }

        public async Task<OperationRequestDto> AddAsync(CreateOperationRequestDto dto)
        {
            var opType = await this._optyperepo.GetByOperationTypeName(dto.OpTypeName);
            var patient = await this._patientrepo.GetPatientByNameAsync(dto.PatientName);
            var doctor = await this._doctorrepo.GetStaffByNameAsync(dto.DoctorName);

            var opRequest = OperationRequestMapper.ToDomain(dto, opType, patient, doctor);

            await this._repo.AddAsync(opRequest);

            await this._unitOfWork.CommitAsync();

            return OperationRequestMapper.ToDto(opRequest);
            //return null;
        }
    }
}