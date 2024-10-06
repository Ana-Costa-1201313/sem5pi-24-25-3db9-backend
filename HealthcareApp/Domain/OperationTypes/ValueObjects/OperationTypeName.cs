using System;
using HealthcareApp.Domain.Shared;
using Microsoft.EntityFrameworkCore;

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
            this.Name = name;
        }

        public void ChangeName(string name)
        {
            this.Name = name;
        }

    }
}