using System;
using Backoffice.Domain.Shared;

namespace Backoffice.Domain.Specializations
{
    public class Specialization : Entity<SpecializationId>, IAggregateRoot
    {

        public virtual SpecializationName Name {get; private set;}

        public bool Active { get; private set; }

        public Specialization()
        {
            this.Active = true;
        }

        public Specialization(SpecializationName name)
        {
            this.Id = new SpecializationId(Guid.NewGuid());
            if (name == null)
            {
                throw new BusinessRuleValidationException("Error: The specialization name can't be null.");
            }
            this.Name = name;
            this.Active = true;
        }

        public void ChangeName(SpecializationName name)
        {
            if (!this.Active)
                throw new BusinessRuleValidationException("Error: It is not possible to change the name of an inactive specialization.");
            this.Name = name;
        }

        public void MarkAsInative()
        {

            if (!this.Active)
            {
                throw new BusinessRuleValidationException("Error: This specialization is already inactive.");
            }
            this.Active = false;
        }
    }
}