using Backoffice.Infraestructure.Shared;
using Backoffice.Domain.OperationRequests;
using Backoffice.Domain.Shared;
using Backoffice.Domain.OperationTypes;
using Backoffice.Domain.Patients;
using Backoffice.Domain.Staffs;
using Microsoft.EntityFrameworkCore;
using Backoffice.Domain.OperationRequests.ValueObjects;

namespace Backoffice.Infraestructure.OperationRequests
{
    public class OperationRequestRepository : BaseRepository<OperationRequest, OperationRequestId>, IOperationRequestRepository
    {
        private readonly BDContext _context;
        public OperationRequestRepository(BDContext context) : base(context.OperationRequests)
        {
            _context = context;
        }

        public async Task<List<OperationRequest>> GetAllOpRequestsAsync()
        {
            return await _context.OperationRequests
                .ToListAsync()
            ;
        }

        public async Task<OperationRequest> GetOpRequestByIdAsync(OperationRequestId id)
        {
            return await _context.OperationRequests
                .Where(x => id.Equals(x.Id)).FirstOrDefaultAsync();
        }

        public async Task<List<OperationRequest>> GetOpRequestsByDoctorEmailAsync(Email doctorEmail)
        {
            return await _context.OperationRequests
                .Where(x => x.DoctorId.Equals(doctorEmail))
                .ToListAsync();
        }
        
        public async Task<List<OperationRequest>> GetOpRequestsByPatientEmailAsDoctorAsync(Email doctorEmail, Email patientEmail)
        {
            return await _context.OperationRequests
                .Where(x => x.DoctorId.Equals(doctorEmail) && x.Patient.FullName.Equals(patientEmail))
                .ToListAsync();
        }

        public async Task<List<OperationRequest>> GetOpRequestsByOperationTypeNameAsDoctorAsync(Email doctorEmail, OperationTypeName operationTypeName)
        {
            return await _context.OperationRequests
                .Where(x => x.DoctorId.Equals(doctorEmail) && x.OpType.Name.Equals(operationTypeName))
                .ToListAsync();
        }

        public async Task<List<OperationRequest>> GetOpRequestsByPriorityAsDoctorAsync(Email doctorEmail, Priority priority)
        {
            return await _context.OperationRequests
                .Where(x => x.DoctorId.Equals(doctorEmail) && x.Priority.Equals(priority))
                .ToListAsync();
        }

        public async Task<List<OperationRequest>> GetOpRequestsByStatusAsDoctorAsync(Email doctorEmail, Status status)
        {
            return await _context.OperationRequests
                .Where(x => x.DoctorId.Equals(doctorEmail) && x.Status.Equals(status))
                .ToListAsync();
        }

        public async Task<List<OperationRequest>> DeleteOpRequestAsync(OperationRequestId id)
        {
            var entity = await _context.OperationRequests
                .Where(x => id.Equals(x.Id))
                .FirstOrDefaultAsync()
            ;

            if (entity != null)
            {
                _context.OperationRequests.Remove(entity);
                await _context.SaveChangesAsync();
            }

            return await _context.OperationRequests
                .ToListAsync()
            ;
        }
    }
}
