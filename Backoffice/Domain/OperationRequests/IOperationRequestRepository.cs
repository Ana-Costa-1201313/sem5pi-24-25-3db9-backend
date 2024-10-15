using Backoffice.Domain.Shared;

namespace Backoffice.Domain.OperationRequests
{
    public interface IOperationRequestRepository : IRepository<OperationRequest, OperationRequestId>
    {

    }
}