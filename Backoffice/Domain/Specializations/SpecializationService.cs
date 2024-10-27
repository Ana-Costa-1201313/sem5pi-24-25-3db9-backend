using System.Threading.Tasks;
using System.Collections.Generic;
using Backoffice.Domain.Shared;

namespace Backoffice.Domain.Specializations
{
    public class SpecializationService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ISpecializationRepository _repo;
        private readonly SpecializationMapper _mapper;

        public SpecializationService(IUnitOfWork unitOfWork, ISpecializationRepository repo, SpecializationMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._repo = repo;
            this._mapper = mapper;
        }

        public async Task<List<SpecializationDto>> GetAllAsync()
        {
            var list = await this._repo.GetAllAsync();

            List<SpecializationDto> listDto = list.ConvertAll<SpecializationDto>(specialization => _mapper.ToDto(specialization)).ToList();

            return listDto;
        }

        public async Task<SpecializationDto> GetByIdAsync(Guid id)
        {
            var spec = await this._repo.GetByIdAsync(new SpecializationId(id));

            if (spec == null)
                return null;

            return _mapper.ToDto(spec);
        }

        public async Task<SpecializationDto> AddAsync(CreatingSpecializationDto dto)
        {
            var spec = _mapper.ToDomain(dto);

            if (await this._repo.SpecializationNameExists(spec.Name.Name))
            {
                throw new BusinessRuleValidationException("Error: This specialization name is already being used.");
            }

            await this._repo.AddAsync(spec);

            await this._unitOfWork.CommitAsync();

            return _mapper.ToDto(spec);
        }

        public async Task<SpecializationDto> InactivateAsync(Guid id)
        {
            var spec = await this._repo.GetByIdAsync(new SpecializationId(id));

            if (spec == null)
                return null;

            spec.MarkAsInative();

            await this._unitOfWork.CommitAsync();

            return _mapper.ToDto(spec);
        }
    }
}