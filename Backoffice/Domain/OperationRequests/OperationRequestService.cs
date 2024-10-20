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
                OpType = or.OpType,
                DeadlineDate = or.DeadlineDate,
                Priority = or.Priority,
                Patient = or.Patient,
                Doctor = or.Doctor
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
                OpType = opReq.OpType,
                DeadlineDate = opReq.DeadlineDate,
                Priority = opReq.Priority,
                Patient = opReq.Patient,
                Doctor = opReq.Doctor
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
                OpType = OperationRequest.OpType,
                DeadlineDate = OperationRequest.DeadlineDate,
                Priority = OperationRequest.Priority,
                Patient = OperationRequest.Patient,
                Doctor = OperationRequest.Doctor
            };
        }
    }
}