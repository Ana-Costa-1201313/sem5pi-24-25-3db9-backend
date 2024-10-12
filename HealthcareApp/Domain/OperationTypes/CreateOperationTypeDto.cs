namespace HealthcareApp.Domain.OperationTypes
{
    public class CreatingOperationTypeDto
    {
        public string Name { get; set; }
        public int AnesthesiaPatientPreparationDuration { get; set; }
        public int SurgeryDuration { get; set; }
        public int CleaningDuration { get; set; }
        public List<RequiredStaffDto> RequiredStaff { get; set; }

        public CreatingOperationTypeDto(string name, int anesthesiaPatientPreparationDuration, 
        int surgeryDuration, int cleaningDuration, List<RequiredStaffDto> requiredStaff)
        {
            this.Name = name;
            this.AnesthesiaPatientPreparationDuration = anesthesiaPatientPreparationDuration;
            this.SurgeryDuration = surgeryDuration;
            this.CleaningDuration = cleaningDuration;
            this.RequiredStaff = requiredStaff;
        }
    }
}