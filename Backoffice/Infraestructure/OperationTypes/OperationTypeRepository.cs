using Backoffice.Domain.Categories;
using Backoffice.Domain.OperationTypes;
using Backoffice.Infraestructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace Backoffice.Infraestructure.Categories
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
        
        public async Task<OperationType> GetByOperationTypeName(string name){
            return await this._context.OperationTypes.Where(x => name.Equals(x.Name.Name)).FirstOrDefaultAsync();
        }

        public async Task<bool> OperationTypeNameExists(string name){
            
            OperationType operationType = await GetByOperationTypeName(name);
            return operationType != null;
        }

    }
}