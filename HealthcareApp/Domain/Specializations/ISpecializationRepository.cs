using HealthcareApp.Domain.Shared;

namespace HealthcareApp.Domain.Specializations
{
    public interface ISpecializationRepository : IRepository<Specialization, SpecializationId>
    {

        public Task<bool> SpecializationNameExists(string name);
        public Task<Specialization> GetBySpecializationName(string name);

    }
}