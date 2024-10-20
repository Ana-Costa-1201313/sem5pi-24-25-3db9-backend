using System.Security.Cryptography;
using Backoffice.Domain.OperationTypes;
using Backoffice.Domain.Patient;
using Backoffice.Domain.Shared;
using Backoffice.Domain.Staff;

namespace Backoffice.Domain.OperationRequests
{
    public class OperationRequest : Entity<OperationRequestId>, IAggregateRoot
    {
        public OperationTypeDto OpType { get; private set; }
        public DateTime DeadlineDate { get; private set; }
        public Priority Priority { get; private set; }
        public PatientDto Patient { get; private set; }
        public StaffDto Doctor { get; private set; }
        // public string OperationRequestId { get; private set; }

        private OperationRequest(){}

        public OperationRequest(CreateOperationRequestDto dto)
        {
            this.Id = new OperationRequestId(Guid.NewGuid());
            this.OpType = dto.OpType;
            this.DeadlineDate = dto.DeadlineDate;
            this.Priority = dto.Priority;
            this.Patient = dto.Patient;
            this.Doctor = dto.Doctor;
        }
    }
}