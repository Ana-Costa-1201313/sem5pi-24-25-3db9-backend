using Backoffice.Domain.Shared;

namespace Backoffice.Domain.OperationRequests
{
    public class OperationRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOperationRequestRepository _repo;

        public OperationRequestService(IUnitOfWork unitOfWork, IOperationRequestRepository repo)
        {
            _unitOfWork = unitOfWork;
            _repo = repo;
        }

        public async Task<List<OperationRequestDto>> GetAllAsync()
        {/*
            var list = await this._repo.GetAllAsync();

            List<OperationRequestDto> listDto = list.ConvertAll<OperationRequestDto>(or => new OperationRequestDto
            {
                Id = or.Id.AsGuid(),
                OpTypeName = or.OpTypeName,
                DeadlineDate = or.DeadlineDate,
                Priority = or.Priority,
                PatientName = or.PatientName,
                DoctorName = or.DoctorName
            });

            return listDto;*/
            return null;
        }

        public async Task<OperationRequestDto> GetByIdAsync(Guid id)
        {/*
            var opReq = await this._repo.GetByIdAsync(new OperationRequestId(id));

            if (opReq == null)
                return null;

            return new OperationRequestDto
            {
                Id = opReq.Id.AsGuid(),
                OpTypeName = opReq.OpTypeName,
                DeadlineDate = opReq.DeadlineDate,
                Priority = opReq.Priority,
                PatientName = opReq.PatientName,
                DoctorName = opReq.DoctorName
            };*/
            return null;
        }

        public async Task<OperationRequestDto> AddAsync(CreateOperationRequestDto dto)
        {/*
            var OperationRequest = new OperationRequest(dto);

            await this._repo.AddAsync(OperationRequest);

            await this._unitOfWork.CommitAsync();

            return new OperationRequestDto
            {
                Id = OperationRequest.Id.AsGuid(),
                OpTypeName = OperationRequest.OpTypeName,
                DeadlineDate = OperationRequest.DeadlineDate,
                Priority = OperationRequest.Priority,
                PatientName = OperationRequest.PatientName,
                DoctorName = OperationRequest.DoctorName
            };*/
            return null;
        }
    }
}