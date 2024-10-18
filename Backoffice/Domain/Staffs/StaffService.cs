using Microsoft.AspNetCore.Mvc;
using Backoffice.Domain.Staffs;
using Backoffice.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Backoffice.Domain.Users;

namespace Backoffice.Domain.Staffs
{
    public class StaffService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStaffRepository _repo;
        private readonly StaffMapper _staffMapper;

        public StaffService(IUnitOfWork unitOfWork, IStaffRepository repo, StaffMapper staffMapper)
        {
            _unitOfWork = unitOfWork;
            _repo = repo;
            _staffMapper = staffMapper;
        }

        public async Task<List<StaffDto>> GetAllAsync()
        {
            var list = await this._repo.GetAllAsync();

            List<StaffDto> listDto = new List<StaffDto>();

            foreach (var staff in list)
            {
                listDto.Add(_staffMapper.ToStaffDto(staff));
            }

            return listDto;
        }

        public async Task<StaffDto> GetByIdAsync(Guid id)
        {
            var staff = await this._repo.GetByIdAsync(new StaffId(id));

            if (staff == null)
                return null;

            return _staffMapper.ToStaffDto(staff);
        }

        public async Task<StaffDto> AddAsync(CreateStaffDto dto)
        {
            int mechanographicNumSeq = await this._repo.GetLastMechanographicNumAsync() + 1;

            var staff = _staffMapper.ToStaff(dto, mechanographicNumSeq);

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

            return _staffMapper.ToStaffDto(staff);
        }

        public async Task<StaffDto> Deactivate(Guid id)
        {
            var staff = await this._repo.GetByIdAsync(new StaffId(id));

            if (staff == null)
            {
                return null;
            }

            staff.Deactivate();

            await this._unitOfWork.CommitAsync();

            return _staffMapper.ToStaffDto(staff);
        }
    }
}
