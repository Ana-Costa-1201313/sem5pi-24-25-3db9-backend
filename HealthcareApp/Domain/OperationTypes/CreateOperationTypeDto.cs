namespace HealthcareApp.Domain.OperationTypes
{
    public class CreatingOperationTypeDto
    {
        public string Description { get; set; }
        public OperationTypeName Name { get; set; }
        public string Duration { get; set; }
        public List<OperationTypeRequiredStaff> RequiredStaff { get; set; }

        public CreatingOperationTypeDto(string description, OperationTypeName name, string duration, List<OperationTypeRequiredStaff> requiredStaff)
        {
            this.Description = description;
            this.Name = name;
            this.Duration = duration;
            this.RequiredStaff = requiredStaff;
        }
    }
}