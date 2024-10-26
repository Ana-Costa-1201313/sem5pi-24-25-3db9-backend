using System.Text.Json.Serialization;
using Backoffice.Domain.OperationRequests.ValueObjects;

namespace Backoffice.Domain.OperationRequests
{
    public class OperationRequestDto
    {
        public Guid Id { get; set; }
        public string OpTypeName { get; set; }
        public string OpTypeId { get; set; }
        public string DeadlineDate { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Priority Priority { get; set; }
        public string PatientName { get; set; }
        public string PatientId { get; set; }
        public string DoctorName { get; set; }
        public string DoctorId { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Status Status { get; set; }
        public string Description { get; set; }
    }
}