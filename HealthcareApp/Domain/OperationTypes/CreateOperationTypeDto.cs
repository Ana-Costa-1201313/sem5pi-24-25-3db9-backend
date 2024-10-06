namespace HealthcareApp.Domain.OperationTypes
{
    public class CreatingOperationTypeDto
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public string Duration { get; set; }
        public List<string> RequiredStaff { get; set; }

        public CreatingOperationTypeDto(string description, string name, string duration, List<string> requiredStaff)
        {
            this.Description = description;
            this.Name = name;
            this.Duration = duration;
            this.RequiredStaff = requiredStaff;
        }
    }
}