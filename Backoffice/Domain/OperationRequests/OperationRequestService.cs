using Backoffice.Domain.Shared;
using Backoffice.Domain.Patients;
using Backoffice.Domain.OperationTypes;
using Backoffice.Domain.Staffs;
using Backoffice.Domain.Logs;
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
        private readonly ILogRepository _logrepo;

        public OperationRequestService(IUnitOfWork unitOfWork, IOperationRequestRepository repo, 
                                        IOperationTypeRepository optyperepo, IPatientRepository patientrepo, 
                                        IStaffRepository doctorrepo, ILogRepository logrepo)
        {
            _unitOfWork = unitOfWork;
            _repo = repo;
            _optyperepo = optyperepo;
            _patientrepo = patientrepo;
            _doctorrepo = doctorrepo;
            _logrepo = logrepo;
        }

        public async Task<List<OperationRequestDto>> GetAllAsync()
        {
            var list = await this._repo.GetAllAsync();

            List<OperationRequestDto> listDto = new List<OperationRequestDto>();
            
            foreach (var item in list)
            {
                OperationTypeName operationTypeName = _optyperepo.GetByIdAsync(item.OpTypeId).Result.Name;
                string patientName = _patientrepo.GetByIdAsync(item.PatientId).Result.FullName.ToString();
                string doctorName = _doctorrepo.GetByIdAsync(item.DoctorId).Result.Name.ToString();

                listDto.Add(OperationRequestMapper.ToDto(item, operationTypeName, patientName, doctorName));
            }

            return listDto;
            //return null;
        }

        public async Task<List<OperationRequestDto>> GetAllByDoctorIdAsync(string doctorId)
        {
            var list = await this._repo.GetOpRequestsByDoctorIdAsync(new StaffId(doctorId));

            List<OperationRequestDto> listDto = new List<OperationRequestDto>();
            
            foreach (var item in list)
            {
                OperationTypeName operationTypeName = _optyperepo.GetByIdAsync(item.OpTypeId).Result.Name;
                string patientName = _patientrepo.GetByIdAsync(item.PatientId).Result.FullName.ToString();
                string doctorName = _doctorrepo.GetByIdAsync(item.DoctorId).Result.Name.ToString();

                listDto.Add(OperationRequestMapper.ToDto(item, operationTypeName, patientName, doctorName));
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

            OperationTypeName operationTypeName = _optyperepo.GetByIdAsync(opReq.OpTypeId).Result.Name;
            string patientName = _patientrepo.GetByIdAsync(opReq.PatientId).Result.FullName.ToString();
            string doctorName = _doctorrepo.GetByIdAsync(opReq.DoctorId).Result.Name.ToString();

            return OperationRequestMapper.ToDto(opReq, operationTypeName, patientName, doctorName);
            //return null;
        }

        public async Task<OperationRequestDto> PickByIdAsync(Guid id)
        {
            var opReq = await this._repo.PickOperationRequestByIdAsync(new OperationRequestId(id));

            if (opReq == null)
                throw new BusinessRuleValidationException("Error: Operation Request not found!");

            OperationTypeName operationTypeName = _optyperepo.GetByIdAsync(opReq.OpTypeId).Result.Name;
            string patientName = _patientrepo.GetByIdAsync(opReq.PatientId).Result.FullName.ToString();
            string doctorName = _doctorrepo.GetByIdAsync(opReq.DoctorId).Result.Name.ToString();

            return OperationRequestMapper.ToDto(opReq, operationTypeName, patientName, doctorName);
            //return null;
        }

        public async Task<OperationRequestDto> AddAsync(CreateOperationRequestDto dto)
        {
            var opType = await this._optyperepo.GetByOperationTypeName(dto.OpTypeName);
            var patient = await this._patientrepo.GetPatientByEmailAsync(new Email(dto.PatientEmail));
            var doctor = await this._doctorrepo.GetStaffByEmailAsync(new Email(dto.DoctorEmail));

            var opRequest = OperationRequestMapper.ToDomain(dto, opType, patient, doctor);

            await this._repo.AddAsync(opRequest);

            OperationTypeName operationTypeName = opType.Name;
            string patientName = patient.FullName.ToString();
            string doctorName = doctor.Name.ToString();

            await this._logrepo.AddAsync(new Log(opRequest.ToJSON(operationTypeName.Name, patientName, doctorName), LogType.Update, LogEntity.OperationRequest, opRequest.Id));
            await this._unitOfWork.CommitAsync();

            return OperationRequestMapper.ToDto(opRequest, operationTypeName, patientName, doctorName);
            //return null;
        }

        public async Task<List<OperationRequestDto>> FilterOperationRequestsAsync(Guid doctorId, Guid patientId, string operationTypeName, string priority, string status)
        {
            Priority prio = Priority.Null;
            Status stat = Status.Null;

            if (priority != null)
            {
                try{
                    prio = (Priority)Enum.Parse(typeof(Priority), priority, true);
                }
                catch (ArgumentException)
                {
                    throw new BusinessRuleValidationException("Error: Invalid Priority value provided!");
                }
            }

            if (status != null)
            {
                try{
                    stat = (Status)Enum.Parse(typeof(Status), status, true);
                }
                catch (ArgumentException)
                {
                    throw new BusinessRuleValidationException("Error: Invalid Status value provided!");
                }
            }

            PatientId patientIdObj = null;

            if (patientId != Guid.Empty)
                patientIdObj = new PatientId(patientId);

            var list = await this._repo.FilterOpRequestsAsync(new StaffId(doctorId), patientIdObj, operationTypeName, prio, stat);

            List<OperationRequestDto> listDto = new List<OperationRequestDto>();
            
            foreach (var item in list)
            {
                OperationTypeName operationTypeNameObj = _optyperepo.GetByIdAsync(item.OpTypeId).Result.Name;
                string patientName = _patientrepo.GetByIdAsync(item.PatientId).Result.FullName.ToString();
                string doctorName = _doctorrepo.GetByIdAsync(item.DoctorId).Result.Name.ToString();

                listDto.Add(OperationRequestMapper.ToDto(item, operationTypeNameObj, patientName, doctorName));
            }

            if (listDto.IsNullOrEmpty())
                throw new BusinessRuleValidationException("No Operation Requests mathcing the criteria found!");

            return listDto;
            //return null;
        }

        public async Task<OperationRequestDto> PatchAsync(Guid id, EditOperationRequestDto dto)
        {
            var opReq = await this._repo.GetByIdAsync(new OperationRequestId(id));

            Priority prio = Priority.Null;

            if (opReq == null)
                throw new BusinessRuleValidationException("Error: Operation Request not found!");

                // Only update DeadlineDate if it's provided
            if (string.IsNullOrEmpty(dto.DeadlineDate))
            {
                dto.DeadlineDate = opReq.DeadlineDate.ToString();
            }

            // Only update Priority if it's provided (default enum value is not null)
            if (dto.Priority == null)
            {
                prio = opReq.Priority;
            } else{
                try{
                    prio = (Priority)Enum.Parse(typeof(Priority), dto.Priority, true);
                }
                catch (ArgumentException)
                {
                    throw new BusinessRuleValidationException("Error: Invalid Priority value provided!");
                }
            }

            // Only update Description if it's provided
            if (string.IsNullOrEmpty(dto.Description))
            {
                dto.Description = opReq.Description;
            }

            var deadlineDate = DateTime.Parse(dto.DeadlineDate);

            OperationTypeName operationTypeName = _optyperepo.GetByIdAsync(opReq.OpTypeId).Result.Name;
            string patientName = _patientrepo.GetByIdAsync(opReq.PatientId).Result.FullName.ToString();
            string doctorName = _doctorrepo.GetByIdAsync(opReq.DoctorId).Result.Name.ToString();

            await this._logrepo.AddAsync(new Log(opReq.ToJSON(operationTypeName.Name, patientName, doctorName), LogType.Update, LogEntity.OperationRequest, opReq.Id));

            opReq.UpdateOperationRequest(deadlineDate, prio, dto.Description);

            await this._unitOfWork.CommitAsync();

            return OperationRequestMapper.ToDto(opReq, operationTypeName, patientName, doctorName);
            //return null;
        }

        public async Task<OperationRequestDto> DeleteAsync(Guid id)
        {
            var opReq = await this._repo.GetByIdAsync(new OperationRequestId(id));

            if (opReq == null)
                throw new BusinessRuleValidationException("Error: Operation Request not found!");

            OperationTypeName operationTypeName = _optyperepo.GetByIdAsync(opReq.OpTypeId).Result.Name;
            string patientName = _patientrepo.GetByIdAsync(opReq.PatientId).Result.FullName.ToString();
            string doctorName = _doctorrepo.GetByIdAsync(opReq.DoctorId).Result.Name.ToString();

            await this._logrepo.AddAsync(new Log(opReq.ToJSON(operationTypeName.Name, patientName, doctorName), LogType.Delete, LogEntity.OperationRequest, opReq.Id));

            await this._repo.DeleteOpRequestAsync(opReq.Id);

            await this._unitOfWork.CommitAsync();

            return OperationRequestMapper.ToDto(opReq, operationTypeName, patientName, doctorName);
            //return null;
        }

        /*
        public async Task<List<OperationRequestDto>> GetAllByPatientIdAsDoctorAsync(string doctorId, string patientId)
        {
            var list = await this._repo.GetOpRequestsByPatientIdAsDoctorAsync(new StaffId(doctorId), new PatientId(patientId));

            List<OperationRequestDto> listDto = new List<OperationRequestDto>();
            
            foreach (var item in list)
            {
                OperationTypeName operationTypeName = _optyperepo.GetByIdAsync(item.OpTypeId).Result.Name;
                string patientName = _patientrepo.GetByIdAsync(item.PatientId).Result.FullName.ToString();
                string doctorName = _doctorrepo.GetByIdAsync(item.DoctorId).Result.Name.ToString();

                listDto.Add(OperationRequestMapper.ToDto(item, operationTypeName, patientName, doctorName));
            }

            if (listDto.IsNullOrEmpty())
                throw new BusinessRuleValidationException("Error: No Operation Requests for the patient with the Id " + patientId + " type found!");

            return listDto;
            //return null;
        }

        public async Task<List<OperationRequestDto>> GetAllByPriorityAsDoctorAsync(string doctorId, string priority)
        {
            var priorityEnum = (Priority)Enum.Parse(typeof(Priority), priority, true);
            var list = await this._repo.GetOpRequestsByPriorityAsDoctorAsync(new StaffId(doctorId), priorityEnum);

            List<OperationRequestDto> listDto = new List<OperationRequestDto>();
            
            foreach (var item in list)
            {
                OperationTypeName operationTypeName = _optyperepo.GetByIdAsync(item.OpTypeId).Result.Name;
                string patientName = _patientrepo.GetByIdAsync(item.PatientId).Result.FullName.ToString();
                string doctorName = _doctorrepo.GetByIdAsync(item.DoctorId).Result.Name.ToString();

                listDto.Add(OperationRequestMapper.ToDto(item, operationTypeName, patientName, doctorName));
            }

            if (listDto.IsNullOrEmpty())
                throw new BusinessRuleValidationException("Error: No Operation Requests with the " + priority + " priority found!");

            return listDto;
            //return null;
        }

        public async Task<List<OperationRequestDto>> GetAllByStatusAsDoctorAsync(string doctorId, string status)
        {
            var statusEnum = (Status)Enum.Parse(typeof(Status), status, true);
            var list = await this._repo.GetOpRequestsByStatusAsDoctorAsync(new StaffId(doctorId), statusEnum);

            List<OperationRequestDto> listDto = new List<OperationRequestDto>();
            
            foreach (var item in list)
            {
                OperationTypeName operationTypeName = _optyperepo.GetByIdAsync(item.OpTypeId).Result.Name;
                string patientName = _patientrepo.GetByIdAsync(item.PatientId).Result.FullName.ToString();
                string doctorName = _doctorrepo.GetByIdAsync(item.DoctorId).Result.Name.ToString();

                listDto.Add(OperationRequestMapper.ToDto(item, operationTypeName, patientName, doctorName));
            }

            if (listDto.IsNullOrEmpty())
                throw new BusinessRuleValidationException("Error: No Operation Requests with the " + status + " status found!");

            return listDto;
            //return null;
        }

        public async Task<List<OperationRequestDto>> GetAllByOpTypeIdAsDoctorAsync(string doctorId, string opTypeId)
        {
            var list = await this._repo.GetOpRequestsByOperationTypeIdAsDoctorAsync(new StaffId(doctorId), new OperationTypeId(opTypeId));

            List<OperationRequestDto> listDto = new List<OperationRequestDto>();
            
            foreach (var item in list)
            {
                OperationTypeName operationTypeName = _optyperepo.GetByIdAsync(item.OpTypeId).Result.Name;
                string patientName = _patientrepo.GetByIdAsync(item.PatientId).Result.FullName.ToString();
                string doctorName = _doctorrepo.GetByIdAsync(item.DoctorId).Result.Name.ToString();

                listDto.Add(OperationRequestMapper.ToDto(item, operationTypeName, patientName, doctorName));
            }

            if (listDto.IsNullOrEmpty())
                throw new BusinessRuleValidationException("Error: No Operation Requests for the " + opTypeId + " type found!");

            return listDto;
            //return null;
        }*/
    }
}