using Backoffice.Domain.Shared;

namespace Backoffice.Domain.Users
{
    public class User : Entity<UserId>, IAggregateRoot
    {
        public Role Role { get; private set; }

        public Email Email { get; private set; }

        public Password Password { get; private set; }

        public bool Active { get; private set; }

        public int MechanographicNum { get; private set; }

        private User()
        {

        }

        public User(Role role, string email, int mechanographicNum)
        {
            this.Id = new UserId(Guid.NewGuid());
            this.Role = role;
            this.MechanographicNum = mechanographicNum;

            if (role != Role.Patient)
            {
                this.Email = new Email(role.ToString().Substring(0, 1) + MechanographicNum  + "@healthcareapp.com");
            }
            else
            {
                this.Email = new Email(email);
            }
            this.Active = false;
        }


        public void ActivateUser(string passwd)
        {
            if (this.Active)
            {
                throw new BusinessRuleValidationException("Error: This user is already active!");
            }
            this.Password = new Password(passwd);

            this.Active = true;
        }
    }
}