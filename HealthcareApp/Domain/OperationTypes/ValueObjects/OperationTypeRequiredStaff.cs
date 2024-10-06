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