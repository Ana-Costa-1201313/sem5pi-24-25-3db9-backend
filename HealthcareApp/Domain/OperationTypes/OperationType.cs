using System;
using HealthcareApp.Domain.Shared;

namespace HealthcareApp.Domain.OperationTypes
{
    public class OperationType : Entity<OperationTypeId>, IAggregateRoot
    {

        public string Description { get; private set; }
        public string Name {get; private set;}
        public string Duration {get; private set;}
        public List<string> RequiredStaff {get; private set;}

        public bool Active { get; private set; }

        private OperationType()
        {
            this.Active = true;
        }

        public OperationType(string description, string name, string duration, List<string> requiredStaff)
        {
            this.Id = new OperationTypeId(Guid.NewGuid());
            this.Description = description;
            this.Name = name;
            this.Duration = duration;
            this.RequiredStaff = requiredStaff;
            this.Active = true;
        }

        public void ChangeDescription(string description)
        {
            if (!this.Active)
                throw new BusinessRuleValidationException("It is not possible to change the description to an inactive operation type.");
            this.Description = description;
        }
        public void MarkAsInative()
        {
            this.Active = false;
        }
    }
}