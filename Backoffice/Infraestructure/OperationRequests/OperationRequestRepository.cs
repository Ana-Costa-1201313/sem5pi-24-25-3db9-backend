using Backoffice.Infraestructure.Shared;
using Backoffice.Domain.OperationRequests;
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

        public async Task<List<OperationRequest>> GetOpRequestsByDoctorIdAsync(StaffId doctorId)
        {
            return await _context.OperationRequests
                .Where(x => x.DoctorId.Equals(doctorId))
                .ToListAsync();
        }
        
        public async Task<List<OperationRequest>> GetOpRequestsByPatientIdAsDoctorAsync(StaffId staffId, PatientId patientId)
        {
            return await _context.OperationRequests
                .Where(x => x.DoctorId.Equals(staffId) && x.PatientId.Equals(patientId))
                .ToListAsync();
        }

        public async Task<List<OperationRequest>> GetOpRequestsByOperationTypeIdAsDoctorAsync(StaffId staffId, OperationTypeId operationTypeId)
        {
            return await _context.OperationRequests
                .Where(x => x.DoctorId.Equals(staffId) && x.OpTypeId.Equals(operationTypeId))
                .ToListAsync();
        }

        public async Task<List<OperationRequest>> GetOpRequestsByPriorityAsDoctorAsync(StaffId staffId, Priority priority)
        {
            return await _context.OperationRequests
                .Where(x => x.DoctorId.Equals(staffId) && x.Priority.Equals(priority))
                .ToListAsync();
        }

        public async Task<List<OperationRequest>> GetOpRequestsByStatusAsDoctorAsync(StaffId staffId, Status status)
        {
            return await _context.OperationRequests
                .Where(x => x.DoctorId.Equals(staffId) && x.Status.Equals(status))
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
