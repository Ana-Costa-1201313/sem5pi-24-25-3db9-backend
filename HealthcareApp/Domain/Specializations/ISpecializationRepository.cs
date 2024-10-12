using HealthcareApp.Domain.Shared;

namespace HealthcareApp.Domain.Specializations
{
    public interface ISpecializationRepository : IRepository<Specialization, SpecializationId>
    {

        public bool SpecializationNameExists(string name);

    }
}