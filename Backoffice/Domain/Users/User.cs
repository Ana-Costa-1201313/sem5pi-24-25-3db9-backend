using Backoffice.Domain.Shared;

namespace Backoffice.Domain.Users
{
    public class User : Entity<UserId>, IAggregateRoot
    {
        public string Username { get; private set; }

        public Role Role { get; private set; }

        public string Email { get; private set; }

        public Password Password { get; private set; }

        public bool Active { get; private set; }

        private User()
        {

        }

        public User(string username, Role role, string email)
        {
            this.Id = new UserId(Guid.NewGuid());
            this.Username = username;
            this.Role = role;
            this.Email = email;
            this.Active = false;
        }

        public void ActivateUser(string passwd)
        {
            if(this.Active) {
                throw new BusinessRuleValidationException("Error: This user is already active!");
            }
            this.Password = new Password(passwd);

            this.Active = true;
        }
    }
}