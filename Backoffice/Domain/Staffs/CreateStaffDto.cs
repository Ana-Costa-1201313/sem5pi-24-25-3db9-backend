using Backoffice.Domain.Shared;
using System.Text.Json.Serialization;

namespace Backoffice.Domain.Staffs
{
    public class CreateStaffDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public int LicenseNumber { get; set; }

        public string Phone { get; set; }

        public string Specialization { get; set; }

        public int AvailabilitySlots { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Role Role { get; set; }

        public int RecruitmentYear { get; set; }
    }
}