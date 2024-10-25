using Backoffice.Domain.Shared;

namespace Backoffice.Domain.OperationTypes
{
    public interface IOperationTypeRepository : IRepository<OperationType, OperationTypeId>
    {
        public Task<List<OperationType>> GetAllWithDetailsAsync();
        public Task<OperationType> GetByIdWithDetailsAsync(OperationTypeId id);
        public Task<bool> OperationTypeNameExists(string name);


    }
}