using Microsoft.AspNetCore.Mvc;
using Backoffice.Domain.Staffs;
using Backoffice.Domain.Shared;
using Backoffice.Domain.Specializations;
using Microsoft.EntityFrameworkCore;
using Backoffice.Domain.Users;
using System.Configuration;
using Backoffice.Domain.Logs;


namespace Backoffice.Domain.Staffs
{
    public class StaffService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStaffRepository _repo;
        private readonly StaffMapper _staffMapper;
        private readonly ISpecializationRepository _specRepo;
        private readonly ILogRepository _logRepo;


        public StaffService(IUnitOfWork unitOfWork, IStaffRepository repo, StaffMapper staffMapper, ISpecializationRepository specRepo, ILogRepository logRepo)
        {
            _unitOfWork = unitOfWork;
            _repo = repo;
            _staffMapper = staffMapper;
            _specRepo = specRepo;
            _logRepo = logRepo;
        }


        public async Task<List<StaffDto>> GetAllAsync()
        {
            List<Staff> list = await this._repo.GetAllWithDetailsAsync();

            List<StaffDto> listDto = new List<StaffDto>();

            foreach (Staff staff in list)
            {
                listDto.Add(_staffMapper.ToStaffDto(staff));
            }

            return listDto;
        }

        public async Task<StaffDto> GetByIdAsync(Guid id)
        {
            Staff staff = await this._repo.GetByIdWithDetailsAsync(new StaffId(id));

            if (staff == null)
                return null;

            return _staffMapper.ToStaffDto(staff);
        }

        public async Task<StaffDto> AddAsync(CreateStaffDto dto)
        {
            int mechanographicNumSeq = await this._repo.GetLastMechanographicNumAsync() + 1;

            if (mechanographicNumSeq == 0)
            {
                throw new BusinessRuleValidationException("Error: Couldn't get the number of staff members!");
            }

            Specialization specialization = null;

            if (dto.Specialization != null)
            {
                specialization = await _specRepo.GetBySpecializationName(dto.Specialization);

                if (specialization == null)
                {
                    throw new BusinessRuleValidationException("Error: There is no specialization with the name " + dto.Specialization + ".");
                }
            }

            Staff staff = _staffMapper.ToStaff(dto, specialization, mechanographicNumSeq, ReadDNS());

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
            var staff = await this._repo.GetByIdWithDetailsAsync(new StaffId(id));

            if (staff == null)
            {
                return null;
            }

            staff.Deactivate();

            await this._logRepo.AddAsync(new Log("The staff with id " + staff.Id.AsGuid() + " was deactivated!", LogType.Deactivate, LogEntity.Staff, staff.Id));

            await this._unitOfWork.CommitAsync();

            return _staffMapper.ToStaffDto(staff);
        }

        public async Task<StaffDto> UpdateAsync(EditStaffDto dto)
        {
            var staff = await _repo.GetByIdWithDetailsAsync(new StaffId(dto.Id));

            if (staff == null)
            {
                return null;
            }

            Specialization specialization = null;

            if (dto.Specialization != null)
            {
                specialization = await _specRepo.GetBySpecializationName(dto.Specialization);

                if (specialization == null)
                {
                    throw new BusinessRuleValidationException("Error: There is no specialization with the name " + dto.Specialization + ".");
                }
            }

            staff.Edit(dto, specialization);

            await this._logRepo.AddAsync(new Log("The staff with id " + staff.Id.AsGuid() + " was edited!", LogType.Update, LogEntity.Staff, staff.Id));

            try
            {
                await this._unitOfWork.CommitAsync();
            }
            catch (DbUpdateException)
            {
                throw new BusinessRuleValidationException("Error: This phone number is already in use!");
            }

            return _staffMapper.ToStaffDto(staff);
        }

        public async Task<StaffDto> PartialUpdateAsync(EditStaffDto dto)
        {
            var staff = await _repo.GetByIdWithDetailsAsync(new StaffId(dto.Id));

            if (staff == null)
            {
                return null;
            }

            Specialization specialization = null;

            if (dto.Specialization != null)
            {
                specialization = await _specRepo.GetBySpecializationName(dto.Specialization);

                if (specialization == null)
                {
                    throw new BusinessRuleValidationException("Error: There is no specialization with the name " + dto.Specialization + ".");
                }
            }

            staff.PartialEdit(dto, specialization);

            await this._logRepo.AddAsync(new Log("The staff with id " + staff.Id.AsGuid() + " was edited!", LogType.Update, LogEntity.Staff, staff.Id));

            try
            {
                await this._unitOfWork.CommitAsync();
            }
            catch (DbUpdateException)
            {
                throw new BusinessRuleValidationException("Error: This phone number is already in use!");
            }

            return _staffMapper.ToStaffDto(staff);
        }

        public virtual string ReadDNS()
        {
            return System.Configuration.ConfigurationManager.AppSettings["DNS_URL"] ?? throw new ConfigurationErrorsException("Error: The DNS is not configured!");
        }
    }
}
