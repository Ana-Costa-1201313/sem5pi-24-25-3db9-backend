using HealthcareApp.Domain.Shared;

namespace HealthcareApp.Domain.Users
{
    public interface IUserRepository : IRepository<User, UserId>
    {

    }
}