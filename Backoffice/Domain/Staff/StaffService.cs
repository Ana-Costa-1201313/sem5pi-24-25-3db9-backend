using Backoffice.Domain.Shared;

namespace Backoffice.Domain.Staff
{
    public class StaffService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStaffRepository _repo;

        public StaffService(IUnitOfWork unitOfWork, IStaffRepository repo) {
            _unitOfWork = unitOfWork;
            _repo = repo;
        }
    }
}