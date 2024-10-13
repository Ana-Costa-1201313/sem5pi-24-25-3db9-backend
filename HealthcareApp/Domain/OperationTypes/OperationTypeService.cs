using System.Threading.Tasks;
using System.Collections.Generic;
using HealthcareApp.Domain.Shared;
using HealthcareApp.Domain.Specializations;

namespace HealthcareApp.Domain.OperationTypes
{
    public class OperationTypeService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IOperationTypeRepository _repo;

        private readonly ISpecializationRepository _repoSpecialization;

        public OperationTypeService(IUnitOfWork unitOfWork, IOperationTypeRepository repo, ISpecializationRepository repoSpecification)
        {
            this._unitOfWork = unitOfWork;
            this._repo = repo;
            this._repoSpecialization = repoSpecification;
        }

        public async Task<List<OperationTypeDto>> GetAllAsync()
        {
            var list = await this._repo.GetAllWithDetailsAsync();

            List<OperationTypeDto> listDto = list.ConvertAll<OperationTypeDto>(opType => new OperationTypeDto
            {
                Id = opType.Id.AsGuid(),
                Name = opType.Name.Name,
                AnesthesiaPatientPreparationInMinutes = (int)opType.Duration.AnesthesiaPatientPreparationInMinutes.TotalMinutes,
                SurgeryInMinutes = (int)opType.Duration.SurgeryInMinutes.TotalMinutes,
                CleaningInMinutes = (int)opType.Duration.CleaningInMinutes.TotalMinutes,
                RequiredStaff = opType.RequiredStaff.ConvertAll<RequiredStaffDto>(staff => new RequiredStaffDto
                {
                    Specialization = staff.Specialization.Name.Name,
                    Total = staff.Total
                })
            });

            return listDto;
        }

        public async Task<OperationTypeDto> GetByIdAsync(OperationTypeId id)
        {
            var opType = await this._repo.GetByIdWithDetailsAsync(id);

            if (opType == null)
                return null;

            return new OperationTypeDto
            {
                Id = opType.Id.AsGuid(),
                Name = opType.Name.Name,
                AnesthesiaPatientPreparationInMinutes = (int)opType.Duration.AnesthesiaPatientPreparationInMinutes.TotalMinutes,
                SurgeryInMinutes = (int)opType.Duration.SurgeryInMinutes.TotalMinutes,
                CleaningInMinutes = (int)opType.Duration.CleaningInMinutes.TotalMinutes,
                RequiredStaff = opType.RequiredStaff.ConvertAll<RequiredStaffDto>(staff => new RequiredStaffDto
                {
                    Specialization = staff.Specialization.Name.Name,
                    Total = staff.Total
                })
            };
        }

        public async Task<OperationTypeDto> AddAsync(CreatingOperationTypeDto dto)
        {

            var requiredStaffList = new List<OperationTypeRequiredStaff>();
            var specializationNamesSet = new HashSet<string>();


            foreach (var staff in dto.RequiredStaff)
            {
                if (!await this._repoSpecialization.SpecializationNameExists(staff.Specialization))
                {
                    throw new BusinessRuleValidationException("Error: There is no specialization with the name " + staff.Specialization + ".");
                }

                // Check for duplicates in specialization names
                if (!specializationNamesSet.Add(staff.Specialization))
                {
                    throw new BusinessRuleValidationException("Error: Can't have duplicate specializations -> " + staff.Specialization + ".");
                }
                var specialization = await _repoSpecialization.GetBySpecializationName(staff.Specialization);
                var requiredStaff = new OperationTypeRequiredStaff(specialization, staff.Total);
                requiredStaffList.Add(requiredStaff);
            }


            var opType = new OperationType(
                new OperationTypeName
                    (
                        dto.Name
                    ),
                new OperationTypeDuration
                    (
                        dto.AnesthesiaPatientPreparationInMinutes,
                        dto.SurgeryInMinutes,
                        dto.CleaningInMinutes
                    ),
                    requiredStaffList
                );

            if (await this._repo.OperationTypeNameExists(opType.Name.Name))
            {
                throw new BusinessRuleValidationException("Error: This operation type name is already being used.");
            }


            await this._repo.AddAsync(opType);

            await this._unitOfWork.CommitAsync();

            return new OperationTypeDto
            {
                Id = opType.Id.AsGuid(),
                Name = opType.Name.Name,
                AnesthesiaPatientPreparationInMinutes = (int)opType.Duration.AnesthesiaPatientPreparationInMinutes.TotalMinutes,
                SurgeryInMinutes = (int)opType.Duration.SurgeryInMinutes.TotalMinutes,
                CleaningInMinutes = (int)opType.Duration.CleaningInMinutes.TotalMinutes,
                RequiredStaff = opType.RequiredStaff.ConvertAll<RequiredStaffDto>(staff => new RequiredStaffDto
                {
                    Specialization = staff.Specialization.Name.Name,
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
                AnesthesiaPatientPreparationInMinutes = (int)opType.Duration.AnesthesiaPatientPreparationInMinutes.TotalMinutes,
                SurgeryInMinutes = (int)opType.Duration.SurgeryInMinutes.TotalMinutes,
                CleaningInMinutes = (int)opType.Duration.CleaningInMinutes.TotalMinutes,
                RequiredStaff = opType.RequiredStaff.ConvertAll<RequiredStaffDto>(staff => new RequiredStaffDto
                {
                    Specialization = staff.Specialization.Name.Name,
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
                AnesthesiaPatientPreparationInMinutes = (int)opType.Duration.AnesthesiaPatientPreparationInMinutes.TotalMinutes,
                SurgeryInMinutes = (int)opType.Duration.SurgeryInMinutes.TotalMinutes,
                CleaningInMinutes = (int)opType.Duration.CleaningInMinutes.TotalMinutes,
                RequiredStaff = opType.RequiredStaff.ConvertAll<RequiredStaffDto>(staff => new RequiredStaffDto
                {
                    Specialization = staff.Specialization.Name.Name,
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
                AnesthesiaPatientPreparationInMinutes = (int)opType.Duration.AnesthesiaPatientPreparationInMinutes.TotalMinutes,
                SurgeryInMinutes = (int)opType.Duration.SurgeryInMinutes.TotalMinutes,
                CleaningInMinutes = (int)opType.Duration.CleaningInMinutes.TotalMinutes,
                RequiredStaff = opType.RequiredStaff.ConvertAll<RequiredStaffDto>(staff => new RequiredStaffDto
                {
                    Specialization = staff.Specialization.Name.Name,
                    Total = staff.Total
                })
            };
        }
    }
}