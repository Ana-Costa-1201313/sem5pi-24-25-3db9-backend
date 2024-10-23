namespace Backoffice.Domain.OperationRequests
{
    public class CreateOperationRequestDto
    {
        public string OpTypeName { get; private set; }
        public string DeadlineDate { get; private set; }
        public string Priority { get; private set; }
        public string PatientName { get; private set; }
        public string DoctorName { get; private set; }

    }
}