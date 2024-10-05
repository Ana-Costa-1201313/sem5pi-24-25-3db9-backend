using HealthcareApp.Domain.Users;
using HealthcareApp.Infraestructure.Shared;

namespace HealthcareApp.Infraestructure.Users
{
    public class UserRepository : BaseRepository<User, UserId>, IUserRepository
    {
        public UserRepository(BDContext context) : base(context.Users)
        {

        }
    }
}