using System;

namespace HealthcareApp.Domain.OperationTypes
{
    public class OperationTypeDto
    {
        public Guid Id { get; set; }

        public string Description { get; set; }
        public OperationTypeName Name { get; set; }
        public string Duration { get; set; }
        public List<OperationTypeRequiredStaff> RequiredStaff { get; set; }
    }
}