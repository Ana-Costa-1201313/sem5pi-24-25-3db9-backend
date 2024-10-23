using System.Security.Cryptography;
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

        public OperationRequest(OperationType operationType, DateTime deadlineDate, Priority priority, 
                                Patient patient, Staff doctor, Status status, string description)
        {
            this.Id = new OperationRequestId(Guid.NewGuid());
            this.OpType = operationType;
            this.OpTypeId = operationType.Id;
            this.DeadlineDate = deadlineDate;
            this.Priority = priority;
            this.Patient = patient;
            this.PatientId = patient.Id;
            this.Doctor = doctor;
            this.DoctorId = doctor.Id;
            this.Status = status;
            this.Description = description;
        }
    }
}