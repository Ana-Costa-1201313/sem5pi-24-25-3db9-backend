using Backoffice.Domain.Shared;
using System.Text.Json.Serialization;

namespace Backoffice.Domain.Staffs
{
    public class StaffDto
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public int LicenseNumber { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Specialization { get; set; }

        public List<string> AvailabilitySlots { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Role Role { get; set; }

        public string MechanographicNum { get; set; }
    }
}