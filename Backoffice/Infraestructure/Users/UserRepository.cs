using Backoffice.Domain.Users;
using Backoffice.Infraestructure.Shared;

namespace Backoffice.Infraestructure.Users
{
    public class UserRepository : BaseRepository<User, UserId>, IUserRepository
    {
        public UserRepository(BDContext context) : base(context.Users)
        {

        }
    }
}