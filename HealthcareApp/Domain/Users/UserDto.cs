using System.Text.Json.Serialization;

namespace HealthcareApp.Domain.Users
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Role Role { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}