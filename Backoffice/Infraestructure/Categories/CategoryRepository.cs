using Backoffice.Domain.Categories;
using Backoffice.Infraestructure.Shared;

namespace Backoffice.Infraestructure.Categories
{
    public class CategoryRepository : BaseRepository<Category, CategoryId>, ICategoryRepository
    {

        public CategoryRepository(BDContext context) : base(context.Categories)
        {

        }


    }
}