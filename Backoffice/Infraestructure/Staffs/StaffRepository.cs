using Backoffice.Domain.Staff;
using Backoffice.Infraestructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace Backoffice.Infraestructure.Staffs
{
    public class StaffRepository : BaseRepository<Staff, StaffId>, IStaffRepository
    {
        private readonly BDContext _context;

        public StaffRepository(BDContext context) : base(context.Staff)
        {
            this._context = context;
        }

        public async Task<int> GetLastMechanographicNumAsync()
        {
            return await _context.Staff
                .OrderByDescending(u => u.MechanographicNum)
                .Select(u => u.MechanographicNum)
                .FirstOrDefaultAsync();
        }
    }
}