using System;
using Backoffice.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace Backoffice.Domain.Specializations
{
    [Owned]
    public class SpecializationName : IValueObject
    {

        public string Name { get; set; }

        private SpecializationName()
        {
        }

        public SpecializationName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new BusinessRuleValidationException("Error: The operation name can't be null, empty or consist in only white spaces.");
            }
            this.Name = name;

        }

    }
}