using Backoffice.Domain.Shared;

namespace Backoffice.Domain.Categories
{
    public interface ICategoryRepository : IRepository<Category, CategoryId>
    {
    }
}