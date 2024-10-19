using Backoffice.Domain.Users;
using Backoffice.Domain.Shared;
using Backoffice.Infraestructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace Backoffice.Infraestructure.Users
{
    public class UserRepository : BaseRepository<User, UserId>, IUserRepository
    {
        private readonly BDContext _context;

        public UserRepository(BDContext context) : base(context.Users)
        {
            _context = context;
        }

        public async Task<int> GetLastMechanographicNumAsync()
        {
            return await _context.Users
                .OrderByDescending(u => u.MechanographicNum)
                .Select(u => u.MechanographicNum)
                .FirstOrDefaultAsync();
        }

        public async Task<User> getUserByEmail(string email)
        {
            Email em = new Email(email);
            return await this._context.Users.Where(x => x.Email.Equals(em)).Select(x => x).FirstOrDefaultAsync();
        }
    }
}