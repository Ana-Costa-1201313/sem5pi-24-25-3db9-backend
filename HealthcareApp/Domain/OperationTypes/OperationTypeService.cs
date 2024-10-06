using System.Threading.Tasks;
using System.Collections.Generic;
using HealthcareApp.Domain.Shared;

namespace HealthcareApp.Domain.OperationTypes
{
    public class OperationTypeService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IOperationTypeRepository _repo;

        public OperationTypeService(IUnitOfWork unitOfWork, IOperationTypeRepository repo)
        {
            this._unitOfWork = unitOfWork;
            this._repo = repo;
        }

        public async Task<List<OperationTypeDto>> GetAllAsync()
        {
            var list = await this._repo.GetAllAsync();

            List<OperationTypeDto> listDto = list.ConvertAll<OperationTypeDto>(opType => new OperationTypeDto { Id = opType.Id.AsGuid(), Description = opType.Description, Name = opType.Name, Duration = opType.Duration, RequiredStaff = opType.RequiredStaff });

            return listDto;
        }

        public async Task<OperationTypeDto> GetByIdAsync(OperationTypeId id)
        {
            var opType = await this._repo.GetByIdAsync(id);

            if (opType == null)
                return null;

            return new OperationTypeDto { Id = opType.Id.AsGuid(), Description = opType.Description, Name = opType.Name, Duration = opType.Duration, RequiredStaff = opType.RequiredStaff };
        }

        public async Task<OperationTypeDto> AddAsync(CreatingOperationTypeDto dto)
        {
            var opType = new OperationType(dto.Description, dto.Name, dto.Duration, dto.RequiredStaff);

            await this._repo.AddAsync(opType);

            await this._unitOfWork.CommitAsync();

            return new OperationTypeDto { Id = opType.Id.AsGuid(), Description = opType.Description, Name = opType.Name, Duration = opType.Duration, RequiredStaff = opType.RequiredStaff };
        }

        public async Task<OperationTypeDto> UpdateAsync(OperationTypeDto dto)
        {
            var opType = await this._repo.GetByIdAsync(new OperationTypeId(dto.Id));

            if (opType == null)
                return null;

            // change all field
            opType.ChangeDescription(dto.Description);

            await this._unitOfWork.CommitAsync();

            return new OperationTypeDto { Id = opType.Id.AsGuid(), Description = opType.Description };
        }

        public async Task<OperationTypeDto> InactivateAsync(OperationTypeId id)
        {
            var opType = await this._repo.GetByIdAsync(id);

            if (opType == null)
                return null;

            // change all fields
            opType.MarkAsInative();

            await this._unitOfWork.CommitAsync();

            return new OperationTypeDto { Id = opType.Id.AsGuid(), Description = opType.Description };
        }

        public async Task<OperationTypeDto> DeleteAsync(OperationTypeId id)
        {
            var opType = await this._repo.GetByIdAsync(id);

            if (opType == null)
                return null;

            if (opType.Active)
                throw new BusinessRuleValidationException("It is not possible to delete an active operation type.");

            this._repo.Remove(opType);
            await this._unitOfWork.CommitAsync();

            return new OperationTypeDto { Id = opType.Id.AsGuid(), Description = opType.Description };
        }
    }
}