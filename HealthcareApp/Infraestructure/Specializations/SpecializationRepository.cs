using HealthcareApp.Domain.Specializations;
using HealthcareApp.Infraestructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace HealthcareApp.Infraestructure.Specializations
{
    public class SpecializationRepository : BaseRepository<Specialization, SpecializationId>, ISpecializationRepository
    {

        private readonly BDContext _context;

        public SpecializationRepository(BDContext context) : base(context.Specializations)
        {
            this._context = context;
        }

        private async Task<Specialization> GetBySpecializationName(string name){
            return await this._context.Specializations.Where(x => name.Equals(x.Name.Name)).FirstOrDefaultAsync();
        }
        
        public bool SpecializationNameExists(string name){
            
            Specialization specialization = GetBySpecializationName(name).Result;
            return specialization != null;
        }


    }
}