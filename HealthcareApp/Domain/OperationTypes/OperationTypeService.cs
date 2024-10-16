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

            List<OperationTypeDto> listDto = list.Select(opType => OperationTypeMapper.ToDto(opType)).ToList();

            return listDto;
        }

        public async Task<OperationTypeDto> GetByIdAsync(Guid id)
        {
            var opType = await this._repo.GetByIdWithDetailsAsync(new OperationTypeId(id));

            if (opType == null)
                return null;

            return OperationTypeMapper.ToDto(opType);
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

            var opType = OperationTypeMapper.ToDomain(dto);

            if (await this._repo.OperationTypeNameExists(opType.Name.Name))
            {
                throw new BusinessRuleValidationException("Error: This operation type name is already being used.");
            }

            await this._repo.AddAsync(opType);

            await this._unitOfWork.CommitAsync();

            return OperationTypeMapper.ToDto(opType);
        }

    }
}