using Backoffice.Domain.Shared;

namespace Backoffice.Domain.Staffs
{
    public interface IStaffRepository : IRepository<Staff, StaffId>
    {
        Task<int> GetLastMechanographicNumAsync();

        Task<Staff> GetStaffByEmailAsync(Email email);

        Task<List<Staff>> GetAllWithDetailsAsync();
        
        Task<Staff> GetByIdWithDetailsAsync(StaffId id);

        Task<List<Staff>> FilterStaffAsync(string name, string email, string specialization);
    }
}