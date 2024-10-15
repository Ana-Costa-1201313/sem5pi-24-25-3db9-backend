using Microsoft.AspNetCore.Mvc;
using Backoffice.Domain.Staff;
using Backoffice.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Backoffice.Domain.Users;

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

        public async Task<List<StaffDto>> GetAllAsync()
        {
            var list = await this._repo.GetAllAsync();

            List<StaffDto> listDto = list.ConvertAll<StaffDto>(s => new StaffDto
            {
                Id = s.Id.AsGuid(),
                FirstName = s.FirstName,
                LastName = s.LastName,
                FullName = s.FullName,
                LicenseNumber = s.LicenseNumber,
                Email = s.Email._Email,
                Phone = s.Phone.PhoneNum,
                Specialization = s.Specialization,
                AvailabilitySlots = s.AvailabilitySlots
            });

            return listDto;
        }

        public async Task<StaffDto> GetByIdAsync(Guid id)
        {
            var staff = await this._repo.GetByIdAsync(new StaffId(id));

            if (staff == null)
                return null;

            return new StaffDto
            {
                Id = staff.Id.AsGuid(),
                FirstName = staff.FirstName,
                LastName = staff.LastName,
                FullName = staff.FullName,
                LicenseNumber = staff.LicenseNumber,
                Email = staff.Email._Email,
                Phone = staff.Phone.PhoneNum,
                Specialization = staff.Specialization,
                AvailabilitySlots = staff.AvailabilitySlots
            };
        }

        public async Task<StaffDto> AddAsync(CreateStaffDto dto)
        {
            var staff = new Staff(dto, await this._repo.GetLastMechanographicNumAsync() + 1);

            try
            {
                await this._repo.AddAsync(staff);

                await this._unitOfWork.CommitAsync();

            }
            catch (DbUpdateException e)
            {

                if (e.InnerException != null && e.InnerException.Message.Contains("UNIQUE constraint failed: Staff.Phone"))
                {
                    throw new BusinessRuleValidationException("Error: This phone number is already in use!");
                }
                if (e.InnerException != null && e.InnerException.Message.Contains("UNIQUE constraint failed: Staff.Email"))
                {
                    throw new BusinessRuleValidationException("Error: This email is already in use!");
                }
                else if (e.InnerException != null && e.InnerException.Message.Contains("UNIQUE constraint failed: Staff.LicenseNumber"))
                {
                    throw new BusinessRuleValidationException("Error: This License number is already in use!");
                }
                else
                {
                    throw new BusinessRuleValidationException("Error: Can't save this data!");
                }
            }

            return new StaffDto
            {
                Id = staff.Id.AsGuid(),
                FirstName = staff.FirstName,
                LastName = staff.LastName,
                FullName = staff.FullName,
                LicenseNumber = staff.LicenseNumber,
                Phone = staff.Phone.PhoneNum,
                Specialization = staff.Specialization,
                AvailabilitySlots = staff.AvailabilitySlots,
                Role = staff.Role,
                Email = staff.Email._Email,
            };
        }
    }
}
