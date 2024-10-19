namespace Backoffice.Domain.OperationTypes
{
    public class PatchOperationTypeDto
    {
        public string? Name { get; set; }
        public int? AnesthesiaPatientPreparationInMinutes { get; set; }
        public int? SurgeryInMinutes { get; set; }
        public int? CleaningInMinutes { get; set; }
        public List<RequiredStaffDto>? RequiredStaff { get; set; }
    }
}