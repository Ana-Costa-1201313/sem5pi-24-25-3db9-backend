namespace Backoffice.Domain.OperationRequests
{
    public class OperationRequestDto
    {
        public Guid Id { get; set; }
        public string OpTypeName { get; set; }
        public string DeadlineDate { get; set; }
        public string Priority { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }

    }
}