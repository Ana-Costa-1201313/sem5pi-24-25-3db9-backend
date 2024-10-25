using Backoffice.Domain.Shared;

namespace Backoffice.Domain.Staffs
{
    public interface IStaffRepository : IRepository<Staff, StaffId>
    {
        Task<int> GetLastMechanographicNumAsync();

        Task<Staff> GetStaffByEmailAsync(Email email);

        public Task<List<Staff>> GetAllWithDetailsAsync();
        
        public Task<Staff> GetByIdWithDetailsAsync(StaffId id);

    }
}