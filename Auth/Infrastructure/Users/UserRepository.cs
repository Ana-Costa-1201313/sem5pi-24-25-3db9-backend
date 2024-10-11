using Auth.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Auth.Infrastructure.Users;


namespace Auth.Infrastructure.Shared
{
    public class UserRepository : BaseRepository<User, UserDb>, IUserRepository
    {

        private readonly AuthDbContext _context;


        public UserRepository(AuthDbContext context) : base(context.Users)
        {
            this._context = context;
        }
        public async Task<User> GetByUsername(string id)
        {
            return await this._context.Users.Where(x => id.Equals(x.username.username)).FirstOrDefaultAsync();
        }

        public bool UserExists(string email)
        {
            User user = GetByUsername(email).Result;
            return !(user == null);
        }



        public async Task<User> GetByEmail(string email)
        {
            //return await this._context.Users.Where(x => email.Equals(x.email.email)).FirstOrDefaultAsync();
            return null;
        }

        public bool EmailExists(string email)
        {
            // User user = GetByEmail(email).Result;
            // if(user==null || email.Equals("eliminado@gmail.com"))
            //     return false;
            // else
                 return true;


        }

    }
}