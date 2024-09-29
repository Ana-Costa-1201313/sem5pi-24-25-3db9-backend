using System.Threading.Tasks;

namespace HealthcareApp.Domain.Shared
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync();
    }
}