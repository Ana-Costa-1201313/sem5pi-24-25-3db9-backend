using System;

namespace HealthcareApp.Domain.OperationTypes
{
    public class OperationTypeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int AnesthesiaPatientPreparationDuration { get; set; }
        public int SurgeryDuration { get; set; }
        public int CleaningDuration { get; set; }
        public List<RequiredStaffDto> RequiredStaff { get; set; }
    }
}