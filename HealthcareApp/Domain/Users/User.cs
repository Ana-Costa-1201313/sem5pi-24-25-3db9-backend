using HealthcareApp.Domain.Shared;

namespace HealthcareApp.Domain.Users
{
    public class User : Entity<UserId>, IAggregateRoot
    {
        public string Username { get; private set; }

        public Role Role { get; private set; }

        public string Email { get; private set; }

        public Password Password { get; private set; }

        private User()
        {

        }

        public User(string username, Role role, string email, Password password)
        {
            this.Id = new UserId(Guid.NewGuid());
            this.Username = username;
            this.Role = role;
            this.Email = email;
            this.Password = password;
        }
    }
}