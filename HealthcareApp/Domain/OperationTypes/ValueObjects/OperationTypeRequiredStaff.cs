using System;
using HealthcareApp.Domain.Shared;
using HealthcareApp.Domain.Specializations;
using Microsoft.EntityFrameworkCore;

namespace HealthcareApp.Domain.OperationTypes
{
    [Owned]
    public class OperationTypeRequiredStaff : IValueObject
    {

        public SpecializationId SpecializationId { get; private set;}
        public Specialization Specialization {get; private set;}
        public int Total {get; private set;}

        private OperationTypeRequiredStaff()
        {
        }

        public OperationTypeRequiredStaff(Specialization specialization, int total)
        {
            this.Specialization = specialization;
            this.SpecializationId = specialization.Id;
            if (total <= 0)
            {
                throw new BusinessRuleValidationException("Error: The total number of required staff of a specialization can't be lower or equal to 0.");
            }
            this.Total = total;
        }

        public void ChangeSpecialization(Specialization specialization)
        {
            this.Specialization = specialization;
        }

        public void ChangeTotal(int total)
        {
            this.Total = total;
        }
    }
}