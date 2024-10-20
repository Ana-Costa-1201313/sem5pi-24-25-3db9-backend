using Backoffice.Domain.OperationTypes;
using Backoffice.Domain.Patient;
using Backoffice.Domain.Staff;

namespace Backoffice.Domain.OperationRequests
{
    public class OperationRequestDto
    {
        public Guid Id { get; set; }
        public OperationTypeDto OpType { get; set; }
        public DateTime DeadlineDate { get; set; }
        public Priority Priority { get; set; }
        public PatientDto Patient { get; set; }
        public StaffDto Doctor { get; set; }

    }
}