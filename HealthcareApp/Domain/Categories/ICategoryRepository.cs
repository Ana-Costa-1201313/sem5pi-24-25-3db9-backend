using HealthcareApp.Domain.Shared;

namespace HealthcareApp.Domain.Categories
{
    public interface ICategoryRepository : IRepository<Category, CategoryId>
    {
    }
}