using System;
using System.ComponentModel.DataAnnotations.Schema;
using Auth.Domain.Shared;

namespace Auth.Domain.Users
{

    public class User : Entity<UserDb>, IAggregateRoot
    {
        private Username _username;

        public Username username { get { return this._username; } }

        public User(string username) {
            _username = new Username(username);    
        }
    }   
}