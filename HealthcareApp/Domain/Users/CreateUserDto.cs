using System.Text.Json.Serialization;

namespace HealthcareApp.Domain.Users
{
    public class CreateUserDto
    {
        public string Username { get; set; }
        
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Role Role { get; set; }
        
        public string Email { get; set; }
        
        public string Password { get; set; }

        public CreateUserDto(string username, Role role, string email, string password)
        {
            this.Username = username;
            this.Role = role;
            this.Email = email;
            this.Password = password;
        }
    }
}