using Backoffice.Controllers;
using Backoffice.Domain.Shared;
using Backoffice.Domain.Staffs;
using Moq;

namespace Backoffice.Tests
{
    public class StaffControllerTest
    {
        private readonly Mock<IStaffRepository> _repo;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly StaffService _service;
        private readonly StaffController _controller;

        public StaffControllerTest()
        {
            _repo = new Mock<IStaffRepository>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _service = new StaffService(_unitOfWork.Object, _repo.Object, new StaffMapper());
            _controller = new StaffController(_service);
        }


    }
}