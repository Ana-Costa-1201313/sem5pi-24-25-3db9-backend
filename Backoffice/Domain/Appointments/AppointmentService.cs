using Backoffice.Domain.OperationRequests;
using Backoffice.Domain.Shared;
using Backoffice.Domain.Staffs;
using Backoffice.Domain.SurgeryRooms;

namespace Backoffice.Domain.Appointments
{
    public class AppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAppointmentRepository _repo;
        private readonly IOperationRequestRepository _opreqrepo;
        private readonly IStaffRepository _staffrepo;
        private readonly ISurgeryRoomRepository _roomrepo;

        //private readonly ILogRepository _logrepo;

        public AppointmentService(IUnitOfWork unitOfWork, IAppointmentRepository repo, 
                                        IOperationRequestRepository opreqrepo, 
                                        IStaffRepository doctorrepo, ISurgeryRoomRepository roomrepo
                                        /*ILogRepository logrepo*/)
        {
            _unitOfWork = unitOfWork;
            _repo = repo;
            _opreqrepo = opreqrepo;
            _roomrepo = roomrepo;
            _staffrepo = doctorrepo;
            //_logrepo = logrepo;
        }

        public async Task<List<AppointmentDto>> GetAllAsync()
        {
            var list = await this._repo.GetAllAsync();

            List<AppointmentDto> listDto = new List<AppointmentDto>();
            
            foreach (var item in list)
            {
                listDto.Add(AppointmentMapper.ToDto(item));
            }

            return listDto;
            //return null;
        }
    }
}