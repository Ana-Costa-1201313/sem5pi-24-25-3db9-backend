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
        {
            var list = await this._repo.GetAllAsync();

            List<OperationRequestDto> listDto = list.ConvertAll<OperationRequestDto>(or => new OperationRequestDto
            {
                Id = or.Id.AsGuid(),
                OperationType = or.OperationType,
                DeadlineDate = or.DeadlineDate,
                Priority = or.Priority,
                PatientId = or.PatientId,
                DoctorId = or.DoctorId
            });

            return listDto;
        }

        public async Task<OperationRequestDto> GetByIdAsync(Guid id)
        {
            var opReq = await this._repo.GetByIdAsync(new OperationRequestId(id));

            if (opReq == null)
                return null;

            return new OperationRequestDto
            {
                Id = opReq.Id.AsGuid(),
                OperationType = opReq.OperationType,
                DeadlineDate = opReq.DeadlineDate,
                Priority = opReq.Priority,
                PatientId = opReq.PatientId,
                DoctorId = opReq.DoctorId
            };
        }

        public async Task<OperationRequestDto> AddAsync(CreateOperationRequestDto dto)
        {
            var OperationRequest = new OperationRequest(dto);

            await this._repo.AddAsync(OperationRequest);

            await this._unitOfWork.CommitAsync();

            return new OperationRequestDto
            {
                Id = OperationRequest.Id.AsGuid(),
                OperationType = OperationRequest.OperationType,
                DeadlineDate = OperationRequest.DeadlineDate,
                Priority = OperationRequest.Priority,
                PatientId = OperationRequest.PatientId,
                DoctorId = OperationRequest.DoctorId
            };
        }
    }
}