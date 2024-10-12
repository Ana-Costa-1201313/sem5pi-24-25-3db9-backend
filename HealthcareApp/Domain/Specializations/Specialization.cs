using System;
using HealthcareApp.Domain.Shared;

namespace HealthcareApp.Domain.Specializations
{
    public class Specialization : Entity<SpecializationId>, IAggregateRoot
    {

        public SpecializationName Name {get; private set;}

        public bool Active { get; private set; }

        private Specialization()
        {
            this.Active = true;
        }

        public Specialization(SpecializationName name)
        {
            this.Id = new SpecializationId(Guid.NewGuid());
            this.Name = name;
            this.Active = true;
        }

        public void ChangeName(SpecializationName name)
        {
            if (!this.Active)
                throw new BusinessRuleValidationException("It is not possible to change the name to an inactive specialization.");
            this.Name = name;
        }

        public void MarkAsInative()
        {
            this.Active = false;
        }
    }
}