using System;
using HealthcareApp.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace HealthcareApp.Domain.OperationTypes
{
    [Owned]
    public class OperationTypeName : IValueObject
    {

        public string Name {get; private set;}

        private OperationTypeName()
        {
        }

        public OperationTypeName(string name)
        {
            
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new BusinessRuleValidationException("Error: The operation type name can't be null, empty or consist in only white spaces.");
            }
            this.Name = name;
        }
    }
}