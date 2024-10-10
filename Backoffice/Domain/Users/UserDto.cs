using System.Text.Json.Serialization;

namespace Backoffice.Domain.Users
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Role Role { get; set; }

        public string Email { get; set; }

        public Password Password { get; set; }

        public bool Active { get; set; }
    }
}