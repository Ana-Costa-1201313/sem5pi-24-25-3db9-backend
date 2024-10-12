using Backoffice.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace Backoffice.Domain.Users
{
    [Owned]
    public class Username
    {
        public string Email { get; private set; }

        private Username()
        {

        }

        public Username(string email)
        {
            if(string.IsNullOrEmpty(email) || !email.Contains("@")) {
                throw new BusinessRuleValidationException("Error: The email is invalid!");
            }
            this.Email = email;
        }

        public override string ToString()
        {
            return this.Email;
        }
    }
}