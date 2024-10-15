using System.Security.Cryptography;
using Backoffice.Domain.Shared;

namespace Backoffice.Domain.OperationRequests
{
    public class OperationRequest : Entity<OperationRequestId>, IAggregateRoot
    {
        public string OperationType { get; private set; }
        public DateTime DeadlineDate { get; private set; }
        public Int32 Priority { get; private set; }
        public string PatientId { get; private set; }
        public string DoctorId { get; private set; }
        public string OperationRequestId { get; private set; }

        private OperationRequest(){}

        public OperationRequest(CreateOperationRequestDto dto)
        {
            this.Id = new OperationRequestId(Guid.NewGuid());
            this.OperationType = dto.OperationType;
            this.DeadlineDate = dto.DeadlineDate;
            this.Priority = dto.Priority;
            this.PatientId = dto.PatientId;
            this.DoctorId = dto.DoctorId;
        }
    }
}