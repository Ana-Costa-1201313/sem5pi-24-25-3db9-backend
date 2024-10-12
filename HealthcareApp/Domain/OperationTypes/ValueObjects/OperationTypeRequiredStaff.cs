using System;
using HealthcareApp.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace HealthcareApp.Domain.OperationTypes
{
    [Owned]
    public class OperationTypeRequiredStaff : IValueObject
    {

        public string Specialization {get; private set;}
        public int Total {get; private set;}

        private OperationTypeRequiredStaff()
        {
        }

        public OperationTypeRequiredStaff(string specialization, int total)
        {
            this.Specialization = specialization;
            if (total <= 0)
            {
                throw new BusinessRuleValidationException("Error: The total number of required staff of a specialization can't be lower or equal to 0.");
            }
            this.Total = total;
        }

        public void ChangeSpecialization(string specialization)
        {
            this.Specialization = specialization;
        }

        public void ChangeTotal(int total)
        {
            this.Total = total;
        }
    }
}