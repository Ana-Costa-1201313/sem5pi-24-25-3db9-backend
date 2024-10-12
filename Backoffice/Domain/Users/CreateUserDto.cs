using System.Text.Json.Serialization;

namespace Backoffice.Domain.Users
{
    public class CreateUserDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Role Role { get; set; }

        public string Username { get; set; }
    }
}