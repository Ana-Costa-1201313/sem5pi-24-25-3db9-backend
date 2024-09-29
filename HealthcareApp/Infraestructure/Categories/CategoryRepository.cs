using HealthcareApp.Domain.Categories;
using HealthcareApp.Infraestructure.Shared;

namespace HealthcareApp.Infraestructure.Categories
{
    public class CategoryRepository : BaseRepository<Category, CategoryId>, ICategoryRepository
    {

        public CategoryRepository(BDContext context) : base(context.Categories)
        {

        }


    }
}