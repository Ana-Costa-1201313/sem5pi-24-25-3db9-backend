using System;
using System.ComponentModel.DataAnnotations.Schema;
using Auth.Domain.Shared;

namespace Auth.Domain.Users
{

    public class User : Entity<UserDb>, IAggregateRoot
    {
        private Username _username;


        public Username username { get { return this._username; } }

        public string role { get; set; }

        public User() { }

        public User(string username)
        {
            _username = new Username(username);
        }

        public User(string username, string role)
        {
            _username = new Username(username);
            this.role = role;
        }
    }
}