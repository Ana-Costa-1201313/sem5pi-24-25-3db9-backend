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
        public Task<List<OperationRequest>> GetOpRequestsByDoctorEmailAsync(Email doctorId);
        public Task<List<OperationRequest>> GetOpRequestsByPatientEmailAsDoctorAsync(Email doctorId, Email patientEmail);
        public Task<List<OperationRequest>> GetOpRequestsByOperationTypeNameAsDoctorAsync(Email doctorId, OperationTypeName operationTypeName);
        public Task<List<OperationRequest>> GetOpRequestsByPriorityAsDoctorAsync(Email doctorId, Priority priority);
        public Task<List<OperationRequest>> GetOpRequestsByStatusAsDoctorAsync(Email doctorId, Status status);
        public Task<List<OperationRequest>> DeleteOpRequestAsync(OperationRequestId id);
    }
}