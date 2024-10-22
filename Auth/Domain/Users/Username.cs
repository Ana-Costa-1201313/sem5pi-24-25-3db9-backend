
using Auth.Domain.Shared;

namespace Auth.Domain.Users
{
    public class Username : ValueObject
    {
        private string _username;

        public string username
        {
            get { return this._username; }
            set { this._username = value; }
        }

        protected Username() { }

        public Username(string? id)
        {
            if (id == null || id == "")
                throw new BusinessRuleValidationException("ERRO: O Nome de Utilizador não pode ser vazio.");
            this._username = id;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var other = (Username)obj;
            return this._username.Equals(other._username);
        }

        public override int GetHashCode()
        {
            return this._username.GetHashCode();
        }
    }
}