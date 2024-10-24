using Backoffice.Domain.Shared;
using Backoffice.Domain.Patients;
using Backoffice.Domain.OperationTypes;
using Backoffice.Domain.Staffs;
using Azure;

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
        {/*
            var list = await this._repo.GetAllAsync();

            List<OperationRequestDto> listDto = new List<OperationRequestDto>();
            
            foreach (var item in list)
            {
                listDto.Add(OperationRequestMapper.ToDto(item));
            }

            return listDto;*/
            return null;
        }

        public async Task<OperationRequestDto> GetByIdAsync(Guid id)
        {/*
            var opReq = await this._repo.GetByIdAsync(new OperationRequestId(id));

            if (opReq == null)
                return null;

            return OperationRequestMapper.ToDto(opReq);*/
            return null;
        }

        public async Task<OperationRequestDto> AddAsync(CreateOperationRequestDto dto)
        {/*
            var OperationRequest = OperationRequestMapper.ToDomain(dto);

            await this._repo.AddAsync(OperationRequest);

            await this._unitOfWork.CommitAsync();

            return OperationRequestMapper.ToDto(OperationRequest);*/
            return null;
        }
    }
}