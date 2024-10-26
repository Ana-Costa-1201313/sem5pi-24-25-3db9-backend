using System.Text.Json.Serialization;

namespace Backoffice.Domain.OperationRequests
{
    public class EditOperationRequestDto
    {
        public string DeadlineDate { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Priority Priority { get; set; }
        
        public string Description { get; set; }
    }
}