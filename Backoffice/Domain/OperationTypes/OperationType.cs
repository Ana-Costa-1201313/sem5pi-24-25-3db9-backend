using System;
using Backoffice.Domain.Shared;

namespace Backoffice.Domain.OperationTypes
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
            if (name == null){
                throw new BusinessRuleValidationException("Error: The operation type name can't be null.");
            }
            this.Name = name;
            if (duration == null){
                throw new BusinessRuleValidationException("Error: The operation type duration can't be null.");
            }
            this.Duration = duration;
            if (!requiredStaff.Any()){
                throw new BusinessRuleValidationException("Error: The operation type must have at least one required staff.");
            }
            this.RequiredStaff = requiredStaff;
            this.Active = true;
        }
        
        public void MarkAsInative()
        {
            this.Active = false;
        }
    }
}