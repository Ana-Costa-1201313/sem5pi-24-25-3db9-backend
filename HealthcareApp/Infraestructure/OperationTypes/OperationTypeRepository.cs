using HealthcareApp.Domain.Categories;
using HealthcareApp.Domain.OperationTypes;
using HealthcareApp.Infraestructure.Shared;

namespace HealthcareApp.Infraestructure.Categories
{
    public class OperationTypeRepository : BaseRepository<OperationType, OperationTypeId>, IOperationTypeRepository
    {

        public OperationTypeRepository(BDContext context) : base(context.OperationTypes)
        {

        }


    }
}