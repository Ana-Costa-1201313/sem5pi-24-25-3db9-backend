using Backoffice.Domain.Shared;

namespace Backoffice.Domain.Users
{
    public class User : Entity<UserId>, IAggregateRoot
    {
        public Role Role { get; private set; }

        public Username Username { get; private set; }

        public Password Password { get; private set; }

        public bool Active { get; private set; }

        //public int MechanographicNum;

        private User()
        {

        }

        public User(Role role, string username)
        {
            //Console.Write("aaaaaaaaaaaaa" + MechanographicNum);

            this.Id = new UserId(Guid.NewGuid());
            this.Role = role;

            if (role != Role.Patient)
            {
                this.Username = new Username(role.ToString().Substring(0, 1) + "1" + "@healthcareapp.com");
            }
            else
            {
                this.Username = new Username(username);
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