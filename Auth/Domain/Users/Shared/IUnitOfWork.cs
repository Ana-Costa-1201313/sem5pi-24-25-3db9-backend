using System.Threading.Tasks;

namespace Auth.Domain.Shared
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync();
    }
}