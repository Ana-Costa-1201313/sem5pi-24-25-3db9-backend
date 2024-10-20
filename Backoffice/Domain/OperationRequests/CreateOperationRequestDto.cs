using Backoffice.Domain.OperationTypes;
using Backoffice.Domain.Patient;
using Backoffice.Domain.Staff;

namespace Backoffice.Domain.OperationRequests
{
    public class CreateOperationRequestDto
    {
        public OperationTypeDto OpType { get; private set; }
        public DateTime DeadlineDate { get; private set; }
        public Priority Priority { get; private set; }
        public PatientDto Patient { get; private set; }
        public StaffDto Doctor { get; private set; }
    }
}