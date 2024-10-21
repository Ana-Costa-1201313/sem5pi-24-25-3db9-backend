using Backoffice.Domain.Staffs;
using Backoffice.Domain.Shared;
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
                .OrderByDescending(u => u.MecNumSequence)
                .Select(u => u.MecNumSequence)
                .FirstOrDefaultAsync();
        }

        public async Task<Staff> GetStaffByEmailAsync(Email email)
        {
            return await _context.Staff
                .Where(s => s.Email == email)
                .Select(s => s)
                .FirstOrDefaultAsync();
        }
    }
}