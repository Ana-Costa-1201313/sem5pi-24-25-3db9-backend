
using System.Threading.Tasks;
using System.Collections.Generic;

using Auth.Domain.Shared;
using Auth.Domain.Users;

namespace Auth.Infrastructure.Users
{

    public interface IUserRepository : IRepository<User, UserDb>
    {
        public bool UserExists(string id);
        public Task<User> GetByUsername(string id);
        

        public bool EmailExists(string email);
        public Task<User> GetByEmail(string email);
    }
}