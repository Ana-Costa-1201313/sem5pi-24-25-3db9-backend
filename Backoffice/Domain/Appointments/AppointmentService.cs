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

        public async Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto)
        {
            var operationRequest = await _opreqrepo.GetOpRequestByIdAsync(new OperationRequestId(dto.OpRequestId));
            if (operationRequest == null) throw new BusinessRuleValidationException("Error: The operation request doesn't exist.");

            if (string.IsNullOrEmpty(dto.StaffIds))
            {
                throw new BusinessRuleValidationException("Error: The staff must be selected.");
            }

            // Split the input string by the semicolon separator
            string[] ids = dto.StaffIds.Split(';');

            // Create a list to hold the parsed GUIDs
            List<StaffId> staffIdList = new List<StaffId>();

            // Iterate through each ID string, parse it to a GUID, and add to the list
            foreach (string id in ids)
            {
                // Trim any leading or trailing whitespace from each ID string
                string trimmedId = id.Trim();

                // Parse the trimmed ID string to a GUID and add to the list
                if (Guid.TryParse(trimmedId, out Guid parsedGuid))
                {
                    staffIdList.Add(new StaffId(parsedGuid));
                }
                else
                {
                    // Optionally, handle the case where parsing fails (e.g., log an error)
                    Console.WriteLine($"Invalid GUID format: {trimmedId}");
                }
            }

            var staffList = await _staffrepo.GetByIdsAsync(staffIdList);

            var surgeryRoom = await _roomrepo.GetSurgeryRoomByIdAsync(new SurgeryRoomId(dto.SurgeryRoomId));
            if (surgeryRoom == null) throw new BusinessRuleValidationException("Error: The surgery room doesn't exist.");

            var appointment = AppointmentMapper.ToDomain(dto, operationRequest, staffList, surgeryRoom);

            await _repo.AddAsync(appointment);
            await _unitOfWork.CommitAsync();

            return AppointmentMapper.ToDto(appointment);
        }
    }
}