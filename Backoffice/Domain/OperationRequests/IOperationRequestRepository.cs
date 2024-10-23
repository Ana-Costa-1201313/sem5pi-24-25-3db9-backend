using Backoffice.Domain.Shared;
using Backoffice.Domain.Staffs;
using Backoffice.Domain.Patients;
using Backoffice.Domain.OperationRequests;
using Backoffice.Domain.OperationTypes;
using Backoffice.Domain.OperationRequests.ValueObjects;

namespace Backoffice.Domain.OperationRequests
{
    public interface IOperationRequestRepository : IRepository<OperationRequest, OperationRequestId>
    {
        public Task<List<OperationRequest>> GetAllOpRequestsAsync();
        public Task<OperationRequest> GetOpRequestByIdAsync(OperationRequestId id);
        public Task<List<OperationRequest>> GetOpRequestsByPatientNameAsDoctorAsync(StaffId doctorId, string patientName);
        public Task<List<OperationRequest>> GetOpRequestsByOperationTypeNameAsDoctorAsync(StaffId doctorId, OperationTypeName operationTypeName);
        public Task<List<OperationRequest>> GetOpRequestsByPriorityAsDoctorAsync(StaffId doctorId, Priority priority);
        public Task<List<OperationRequest>> GetOpRequestsByStatusAsDoctorAsync(StaffId doctorId, Status status);
    }
}