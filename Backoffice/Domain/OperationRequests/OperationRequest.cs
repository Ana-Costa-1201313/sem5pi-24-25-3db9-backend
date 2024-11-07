using Newtonsoft.Json;
using Backoffice.Domain.OperationRequests.ValueObjects;
using Backoffice.Domain.OperationTypes;
using Backoffice.Domain.Patients;
using Backoffice.Domain.Shared;
using Backoffice.Domain.Staffs;

namespace Backoffice.Domain.OperationRequests
{
    public class OperationRequest : Entity<OperationRequestId>, IAggregateRoot
    {
        public OperationType OpType { get; private set; }
        public OperationTypeId OpTypeId { get; private set; }
        public DateTime DeadlineDate { get; private set; }
        public Priority Priority { get; private set; }
        public Patient Patient { get; private set; }
        public PatientId PatientId { get; private set; }
        public Staff Doctor { get; private set; }
        public StaffId DoctorId { get; private set; }
        public Status Status { get; private set; }
        public string Description { get; private set; }
        // public string OperationRequestId { get; private set; }

        private OperationRequest(){}

        public OperationRequest(OperationType operationType, DateTime deadlineDate, Priority? priority, 
                                Patient patient, Staff doctor, string description)
        {
            this.Id = new OperationRequestId(Guid.NewGuid());

            if (operationType == null)
            {
                this.OpTypeId = null;
                throw new BusinessRuleValidationException("Error: The operation type can't be null.");
            }
            this.OpType = operationType;
            this.OpTypeId = operationType.Id;

            if (deadlineDate == DateTime.MinValue)
            {
                throw new BusinessRuleValidationException("Error: The deadline date can't be the default value.");
            }
            this.DeadlineDate = deadlineDate;

            if (!priority.HasValue)
            {
                throw new BusinessRuleValidationException("Error: The priority can't be null.");
            }
            this.Priority = priority.Value;

            if (patient == null)
            {
                this.PatientId = null;
                throw new BusinessRuleValidationException("Error: The patient can't be null.");
            }
            this.Patient = patient;
            this.PatientId = patient.Id;

            if (doctor == null)
            {
                this.DoctorId = null;
                throw new BusinessRuleValidationException("Error: The doctor can't be null.");
            }
            this.Doctor = doctor;
            this.DoctorId = doctor.Id;

            this.Status = Status.Requested;

            if (string.IsNullOrWhiteSpace(description))
            {
                throw new BusinessRuleValidationException("Error: The description can't be null or empty.");
            }
            this.Description = description;
        }

        public void ChangeStatus()
        {
            this.Status = Status.Picked;
        }

        public void UpdateOperationRequest(DateTime deadlineDate, Priority? priority, string description)
        {
            if (deadlineDate == DateTime.MinValue)
            {
                throw new BusinessRuleValidationException("Error: The deadline date can't be the default value.");
            }
            this.DeadlineDate = deadlineDate;

            if (!priority.HasValue)
            {
                throw new BusinessRuleValidationException("Error: The priority can't be null.");
            }
            this.Priority = priority.Value;

            if (string.IsNullOrWhiteSpace(description))
            {
                throw new BusinessRuleValidationException("Error: The description can't be null or empty.");
            }
            this.Description = description;
        }

        public string ToJSON(string optypename, string patientname, string doctorname)
        {
            var jsonRepresentation = new
            {
                Id = this.Id.Value,
                OperationType = optypename,  // Pass the name object directly
                DeadlineDate = this.DeadlineDate,
                Priority = this.Priority.ToString(),
                Patient = patientname,  // Pass the name object directly
                Doctor = doctorname,  // Pass the name object directly
                Status = this.Status.ToString(),
                Description = this.Description
            };

            return JsonConvert.SerializeObject(jsonRepresentation, Formatting.Indented);
        }

        // TODO: update OperationRequest related acceptance criteria
        // TODO: update operation request related tests
        // TODO: handle DateTime format
        // TODO: handle string with staffIds format
        // TODO: controller sending HTTP requests to other controllers?
    }
}