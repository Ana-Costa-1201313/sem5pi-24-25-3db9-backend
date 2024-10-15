using Backoffice.Domain.OperationRequests;
using Backoffice.Infraestructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace Backoffice.Infraestructure.OperationRequests
{
    public class OperationRequestRepository : BaseRepository<OperationRequest, OperationRequestId>, IOperationRequestRepository
    {
        public OperationRequestRepository(BDContext context) : base(context.OperationRequests)
        {
        }
    }
}