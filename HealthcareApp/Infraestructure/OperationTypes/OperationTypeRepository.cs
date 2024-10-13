using HealthcareApp.Domain.Categories;
using HealthcareApp.Domain.OperationTypes;
using HealthcareApp.Infraestructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace HealthcareApp.Infraestructure.Categories
{
    public class OperationTypeRepository : BaseRepository<OperationType, OperationTypeId>, IOperationTypeRepository
    {

        private readonly BDContext _context;
        public OperationTypeRepository(BDContext context) : base(context.OperationTypes)
        {
            _context = context;
        }

        public async Task<List<OperationType>> GetAllWithDetailsAsync()
        {
            return await _context.OperationTypes
            .Include(rs => rs.RequiredStaff)
            .ThenInclude(s => s.Specialization)
            .ToListAsync();      
        }

        public async Task<OperationType> GetByIdWithDetailsAsync(OperationTypeId id)
        {
            return await _context.OperationTypes
            .Include(rs => rs.RequiredStaff)
            .ThenInclude(s => s.Specialization)
            .FirstOrDefaultAsync(rs => rs.Id == id);      
        }
    }
}