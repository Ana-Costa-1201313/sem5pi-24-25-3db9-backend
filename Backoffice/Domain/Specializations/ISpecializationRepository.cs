using Backoffice.Domain.Shared;

namespace Backoffice.Domain.Specializations
{
    public interface ISpecializationRepository : IRepository<Specialization, SpecializationId>
    {

        public Task<bool> SpecializationNameExists(string name);
        public Task<Specialization> GetBySpecializationName(string name);

    }
}