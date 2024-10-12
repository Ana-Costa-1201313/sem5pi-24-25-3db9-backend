using System;
using HealthcareApp.Domain.Shared;

namespace HealthcareApp.Domain.OperationTypes
{
    public class OperationType : Entity<OperationTypeId>, IAggregateRoot
    {

        public OperationTypeName Name {get; private set;}
        public OperationTypeDuration Duration {get; private set;}
        public List<OperationTypeRequiredStaff> RequiredStaff {get; private set;}

        public bool Active { get; private set; }

        private OperationType()
        {
            this.Active = true;
        }

        public OperationType(OperationTypeName name, OperationTypeDuration duration, List<OperationTypeRequiredStaff> requiredStaff)
        {
            this.Id = new OperationTypeId(Guid.NewGuid());
            this.Name = name;
            this.Duration = duration;
            this.RequiredStaff = requiredStaff;
            this.Active = true;
        }

        public void ChangeDescription(OperationTypeDuration duration)
        {
            if (!this.Active)
                throw new BusinessRuleValidationException("It is not possible to change the description to an inactive operation type.");
            this.Duration = duration;
        }
        public void MarkAsInative()
        {
            this.Active = false;
        }
    }
}