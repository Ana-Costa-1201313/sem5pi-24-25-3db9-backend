using Backoffice.Domain.Shared;

namespace Backoffice.Domain.Staff
{
    public class StaffService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStaffRepository _repo;

        public StaffService(IUnitOfWork unitOfWork, IStaffRepository repo)
        {
            _unitOfWork = unitOfWork;
            _repo = repo;
        }

        public async Task<StaffDto> AddAsync(CreateStaffDto dto)
        {
            var staff = new Staff(dto);

            await this._repo.AddAsync(staff);

            await this._unitOfWork.CommitAsync();

            return new StaffDto
            {
                Id = staff.Id.AsGuid(),
                FirstName = staff.FirstName,
                LastName = staff.LastName,
                FullName = staff.FullName,
                LicenseNumber = staff.LicenseNumber,
                Email = staff.Email,
                Phone = staff.Phone,
                Specialization = staff.Specialization,
                AvailabilitySlots = staff.AvailabilitySlots
            };
        }
    }
}