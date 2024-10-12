using System.Threading.Tasks;
using System.Collections.Generic;
using HealthcareApp.Domain.Shared;

namespace HealthcareApp.Domain.Specializations
{
    public class SpecializationService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ISpecializationRepository _repo;

        public SpecializationService(IUnitOfWork unitOfWork, ISpecializationRepository repo)
        {
            this._unitOfWork = unitOfWork;
            this._repo = repo;
        }

        public async Task<List<SpecializationDto>> GetAllAsync()
        {
            var list = await this._repo.GetAllAsync();

            List<SpecializationDto> listDto = list.ConvertAll<SpecializationDto>(specialization => new SpecializationDto
            {
                Id = specialization.Id.AsGuid(),
                Name = specialization.Name.Name
            });

            return listDto;
        }

        public async Task<SpecializationDto> GetByIdAsync(SpecializationId id)
        {
            var opType = await this._repo.GetByIdAsync(id);

            if (opType == null)
                return null;

            return new SpecializationDto
            {
                Id = opType.Id.AsGuid(),
                Name = opType.Name.Name
            };
        }

        public async Task<SpecializationDto> AddAsync(CreatingSpecializationDto dto)
        {
            var spec = new Specialization(
                new SpecializationName
                    (
                        dto.Name
                    )
                );

            if (this._repo.SpecializationNameExists(spec.Name.Name))
            {
                throw new BusinessRuleValidationException("Error: This specialization name is already being used.");
            }

            await this._repo.AddAsync(spec);

            await this._unitOfWork.CommitAsync();

            return new SpecializationDto
            {
                Id = spec.Id.AsGuid(),
                Name = spec.Name.Name
            };
        }

        public async Task<SpecializationDto> UpdateAsync(SpecializationDto dto)
        {
            var spec = await this._repo.GetByIdAsync(new SpecializationId(dto.Id));

            if (spec == null)
                return null;

            // change all field
            spec.ChangeName(spec.Name);

            await this._unitOfWork.CommitAsync();

            return new SpecializationDto
            {
                Id = spec.Id.AsGuid(),
                Name = spec.Name.Name
            };
        }

        public async Task<SpecializationDto> InactivateAsync(SpecializationId id)
        {
            var spec = await this._repo.GetByIdAsync(id);

            if (spec == null)
                return null;

            // change all fields
            spec.MarkAsInative();

            await this._unitOfWork.CommitAsync();

            return new SpecializationDto
            {
                Id = spec.Id.AsGuid(),
                Name = spec.Name.Name
            };
        }
    }
}