using System;
using Backoffice.Domain.Shared;
using Newtonsoft.Json;

namespace Backoffice.Domain.OperationTypes
{
    public class OperationType : Entity<OperationTypeId>, IAggregateRoot
    {

        public virtual OperationTypeName Name { get; private set; }
        public virtual OperationTypeDuration Duration { get; private set; }
        public virtual List<OperationTypeRequiredStaff> RequiredStaff { get; private set; }

        public virtual bool Active { get; private set; }

        public OperationType()
        {
            this.Active = true;
        }

        public OperationType(OperationTypeName name, OperationTypeDuration duration, List<OperationTypeRequiredStaff> requiredStaff)
        {
            this.Id = new OperationTypeId(Guid.NewGuid());
            if (name == null)
            {
                throw new BusinessRuleValidationException("Error: The operation type name can't be null.");
            }
            this.Name = name;
            if (duration == null)
            {
                throw new BusinessRuleValidationException("Error: The operation type duration can't be null.");
            }
            this.Duration = duration;
            if (requiredStaff == null)
            {
                throw new BusinessRuleValidationException("Error: The operation type can't have null required staff.");
            }
            if (!requiredStaff.Any())
            {
                throw new BusinessRuleValidationException("Error: The operation type must have at least one required staff.");
            }
            this.RequiredStaff = requiredStaff;
            this.Active = true;
        }

        public void MarkAsInative()
        {
            if (!this.Active)
            {
                throw new BusinessRuleValidationException("Error: The operation type is already inactive.");
            }
            this.Active = false;
        }

        public string ToJSON()
        {
            var jsonRepresentation = new
            {
                Id = this.Id.Value,
                Name = new { this.Name.Name },
                Duration = new
                {
                    AnesthesiaPatientPreparationInMinutes = this.Duration.AnesthesiaPatientPreparationInMinutes,
                    SurgeryInMinutes = this.Duration.SurgeryInMinutes,
                    CleaningInMinutes = this.Duration.CleaningInMinutes
                },
                RequiredStaff = this.RequiredStaff.Select(rs => new
                {
                    Specialization = new { rs.Specialization.Name.Name },
                    Total = rs.Total
                }).ToList(),
                Active = this.Active
            };

            return JsonConvert.SerializeObject(jsonRepresentation, Formatting.Indented);
        }
    }
}