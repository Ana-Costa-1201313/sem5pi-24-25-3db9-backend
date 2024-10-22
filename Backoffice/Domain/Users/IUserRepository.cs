using Backoffice.Domain.Shared;

namespace Backoffice.Domain.Users
{
    public interface IUserRepository : IRepository<User, UserId>
    {
    Task<int> GetLastMechanographicNumAsync();
        Task<User> getUserByEmail(string email);
    }
}