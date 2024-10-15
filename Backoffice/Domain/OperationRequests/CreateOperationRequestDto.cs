namespace Backoffice.Domain.OperationRequests
{
    public class CreateOperationRequestDto
    {
        public DateTime DeadlineDate { get; private set; }
        public Int32 Priority { get; private set; }
    }
}