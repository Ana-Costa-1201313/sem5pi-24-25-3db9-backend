using HealthcareApp.Domain.Shared;

namespace HealthcareApp.Domain.OperationTypes
{
    public interface IOperationTypeRepository : IRepository<OperationType, OperationTypeId>
    {
    }
}