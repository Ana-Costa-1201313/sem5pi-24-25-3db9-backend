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

            List<OperationTypeDto> listDto = list.ConvertAll<OperationTypeDto>(opType => new OperationTypeDto
            {
                Id = opType.Id.AsGuid(),
                Name = opType.Name.Name,
                AnesthesiaPatientPreparationDuration = (int)opType.Duration.AnesthesiaPatientPreparationInMinutes.TotalMinutes,
                SurgeryDuration = (int)opType.Duration.SurgeryInMinutes.TotalMinutes,
                CleaningDuration = (int)opType.Duration.CleaningInMinutes.TotalMinutes,
                RequiredStaff = opType.RequiredStaff.ConvertAll<RequiredStaffDto>(staff => new RequiredStaffDto
                {
                    Specialization = staff.Specialization,
                    Total = staff.Total
                })
            });

            return listDto;
        }

        public async Task<OperationTypeDto> GetByIdAsync(OperationTypeId id)
        {
            var opType = await this._repo.GetByIdAsync(id);

            if (opType == null)
                return null;

            return new OperationTypeDto
            {
                Id = opType.Id.AsGuid(),
                Name = opType.Name.Name,
                AnesthesiaPatientPreparationDuration = (int)opType.Duration.AnesthesiaPatientPreparationInMinutes.TotalMinutes,
                SurgeryDuration = (int)opType.Duration.SurgeryInMinutes.TotalMinutes,
                CleaningDuration = (int)opType.Duration.CleaningInMinutes.TotalMinutes,
                RequiredStaff = opType.RequiredStaff.ConvertAll<RequiredStaffDto>(staff => new RequiredStaffDto
                {
                    Specialization = staff.Specialization,
                    Total = staff.Total
                })
            };
        }

        public async Task<OperationTypeDto> AddAsync(CreatingOperationTypeDto dto)
        {
            var opType = new OperationType(
                new OperationTypeName
                    (
                        dto.Name
                    ),
                new OperationTypeDuration
                    (
                        dto.AnesthesiaPatientPreparationDuration,
                        dto.SurgeryDuration,
                        dto.CleaningDuration
                    ),
                dto.RequiredStaff.ConvertAll(staff => new OperationTypeRequiredStaff
                    (
                        staff.Specialization,  
                        staff.Total      
                    ))
                );

            await this._repo.AddAsync(opType);

            await this._unitOfWork.CommitAsync();

            return new OperationTypeDto
            {
                Id = opType.Id.AsGuid(),
                Name = opType.Name.Name,
                AnesthesiaPatientPreparationDuration = (int)opType.Duration.AnesthesiaPatientPreparationInMinutes.TotalMinutes,
                SurgeryDuration = (int)opType.Duration.SurgeryInMinutes.TotalMinutes,
                CleaningDuration = (int)opType.Duration.CleaningInMinutes.TotalMinutes,
                RequiredStaff = opType.RequiredStaff.ConvertAll<RequiredStaffDto>(staff => new RequiredStaffDto
                {
                    Specialization = staff.Specialization,
                    Total = staff.Total
                })
            };
        }

        public async Task<OperationTypeDto> UpdateAsync(OperationTypeDto dto)
        {
            var opType = await this._repo.GetByIdAsync(new OperationTypeId(dto.Id));

            if (opType == null)
                return null;

            // change all field
            opType.ChangeDescription(opType.Duration);

            await this._unitOfWork.CommitAsync();

            return new OperationTypeDto
            {
                Id = opType.Id.AsGuid(),
                Name = opType.Name.Name,
                AnesthesiaPatientPreparationDuration = (int)opType.Duration.AnesthesiaPatientPreparationInMinutes.TotalMinutes,
                SurgeryDuration = (int)opType.Duration.SurgeryInMinutes.TotalMinutes,
                CleaningDuration = (int)opType.Duration.CleaningInMinutes.TotalMinutes,
                RequiredStaff = opType.RequiredStaff.ConvertAll<RequiredStaffDto>(staff => new RequiredStaffDto
                {
                    Specialization = staff.Specialization,
                    Total = staff.Total
                })
            };
        }

        public async Task<OperationTypeDto> InactivateAsync(OperationTypeId id)
        {
            var opType = await this._repo.GetByIdAsync(id);

            if (opType == null)
                return null;

            // change all fields
            opType.MarkAsInative();

            await this._unitOfWork.CommitAsync();

            return new OperationTypeDto
            {
                Id = opType.Id.AsGuid(),
                Name = opType.Name.Name,
                AnesthesiaPatientPreparationDuration = (int)opType.Duration.AnesthesiaPatientPreparationInMinutes.TotalMinutes,
                SurgeryDuration = (int)opType.Duration.SurgeryInMinutes.TotalMinutes,
                CleaningDuration = (int)opType.Duration.CleaningInMinutes.TotalMinutes,
                RequiredStaff = opType.RequiredStaff.ConvertAll<RequiredStaffDto>(staff => new RequiredStaffDto
                {
                    Specialization = staff.Specialization,
                    Total = staff.Total
                })
            };
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

            return new OperationTypeDto
            {
                Id = opType.Id.AsGuid(),
                Name = opType.Name.Name,
                AnesthesiaPatientPreparationDuration = (int)opType.Duration.AnesthesiaPatientPreparationInMinutes.TotalMinutes,
                SurgeryDuration = (int)opType.Duration.SurgeryInMinutes.TotalMinutes,
                CleaningDuration = (int)opType.Duration.CleaningInMinutes.TotalMinutes,
                RequiredStaff = opType.RequiredStaff.ConvertAll<RequiredStaffDto>(staff => new RequiredStaffDto
                {
                    Specialization = staff.Specialization,
                    Total = staff.Total
                })
            };
        }
    }
}