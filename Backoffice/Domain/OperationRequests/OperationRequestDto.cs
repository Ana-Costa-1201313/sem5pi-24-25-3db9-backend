namespace Backoffice.Domain.OperationRequests
{
    public class OperationRequestDto
    {
        public Guid Id { get; set; }
        public string OperationType { get; private set; }
        public DateTime DeadlineDate { get; private set; }
        public Int32 Priority { get; private set; }
        public string PatientId { get; private set; }
        public string DoctorId { get; private set; }

    }
}