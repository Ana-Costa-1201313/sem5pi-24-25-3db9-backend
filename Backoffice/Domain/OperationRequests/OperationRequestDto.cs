namespace Backoffice.Domain.OperationRequests
{
    public class OperationRequestDto
    {
        public Guid Id { get; set; }
        public DateTime DeadlineDate { get; private set; }
        public Int32 Priority { get; private set; }
    }
}