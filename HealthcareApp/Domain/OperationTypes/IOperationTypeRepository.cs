using HealthcareApp.Domain.Shared;

namespace HealthcareApp.Domain.OperationTypes
{
    public interface IOperationTypeRepository : IRepository<OperationType, OperationTypeId>
    {
        public Task<List<OperationType>> GetAllWithDetailsAsync();
        public Task<OperationType> GetByIdWithDetailsAsync(OperationTypeId id);

    }
}