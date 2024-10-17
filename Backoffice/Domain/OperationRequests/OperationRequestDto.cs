namespace Backoffice.Domain.OperationRequests
{
    public class OperationRequestDto
    {
        public Guid Id { get; set; }
        public string OperationType { get; set; }
        public DateTime DeadlineDate { get; set; }
        public Int32 Priority { get; set; }
        public string PatientId { get; set; }
        public string DoctorId { get; set; }

    }
}