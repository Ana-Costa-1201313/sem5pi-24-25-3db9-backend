using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Backoffice.Controllers;
using Backoffice.Domain.OperationTypes;
using Backoffice.Domain.Shared;
using Backoffice.Domain.Specializations;
using Microsoft.AspNetCore.Mvc;
using Xunit.Abstractions;

namespace Backoffice.Tests.Controllers
{
    public class OperationTypesControllerTests
    {
        private readonly Mock<IOperationTypeRepository> _mockRepo;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ISpecializationRepository> _mockSpecializationRepo;
        private readonly OperationTypesController _controller;
        private readonly OperationTypeService _service;

        public OperationTypesControllerTests()
        {
            _mockRepo = new Mock<IOperationTypeRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockSpecializationRepo = new Mock<ISpecializationRepository>();
            _service = new OperationTypeService(
                _mockUnitOfWork.Object,
                _mockRepo.Object,
                _mockSpecializationRepo.Object
            );
            _controller = new OperationTypesController(_service);
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WithOperationType()
        {

            var requiredStaff = new List<(string SpecializationName, int Total)>
                {
                    ("Surgeon", 5)
                };

            var operationType = OperationTypeMapper.ToDomainForTests("Surgery", 30, 60, 15, requiredStaff);


            _mockRepo.Setup(repo => repo.GetByIdWithDetailsAsync(It.IsAny<OperationTypeId>()))
                     .ReturnsAsync(operationType);

            var result = await _controller.GetGetById(operationType.Id.AsGuid());

            var okResult = Assert.IsType<ActionResult<OperationTypeDto>>(result);
            var returnValue = Assert.IsType<OperationTypeDto>(okResult.Value);

            Assert.Equal("Surgery", returnValue.Name);
            Assert.Equal(30, returnValue.AnesthesiaPatientPreparationInMinutes);
            Assert.Equal(60, returnValue.SurgeryInMinutes);
            Assert.Equal(15, returnValue.CleaningInMinutes);
            Assert.Single(returnValue.RequiredStaff);
            Assert.Equal("Surgeon", returnValue.RequiredStaff[0].Specialization);
            Assert.Equal(5, returnValue.RequiredStaff[0].Total);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithOperationTypes()
        {

            var requiredStaff1 = new List<(string SpecializationName, int Total)>
                {
                    ("Surgeon", 5)
                };
            var operationType1 = OperationTypeMapper.ToDomainForTests("Surgery",30,60,15, requiredStaff1);


            var requiredStaff2 = new List<(string SpecializationName, int Total)>
                {
                    ("Surgeon", 2),
                    ("Cardio", 3)
                };
            var operationType2 = OperationTypeMapper.ToDomainForTests("Embolectomy",30,60,15, requiredStaff2);


            var listOp = new List<OperationType> { operationType1, operationType2 };

            _mockRepo.Setup(repo => repo.GetAllWithDetailsAsync())
                     .ReturnsAsync(listOp);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<ActionResult<IEnumerable<OperationTypeDto>>>(result);
            var returnValue = Assert.IsType<List<OperationTypeDto>>(okResult.Value);

            Assert.Equal(2, returnValue.Count);

            Assert.Equal("Surgery", returnValue[0].Name);
            Assert.Equal(30, returnValue[0].AnesthesiaPatientPreparationInMinutes);
            Assert.Equal(60, returnValue[0].SurgeryInMinutes);
            Assert.Equal(15, returnValue[0].CleaningInMinutes);
            Assert.Single(returnValue[0].RequiredStaff);


            var surgeryStaff = returnValue[0].RequiredStaff.First();
            Assert.Equal("Surgeon", surgeryStaff.Specialization);
            Assert.Equal(5, surgeryStaff.Total);

            Assert.Equal("Embolectomy", returnValue[1].Name);
            Assert.Equal(30, returnValue[1].AnesthesiaPatientPreparationInMinutes);
            Assert.Equal(60, returnValue[1].SurgeryInMinutes);
            Assert.Equal(15, returnValue[1].CleaningInMinutes);
            Assert.Equal(2, returnValue[1].RequiredStaff.Count);

            var embolectomyStaff1 = returnValue[1].RequiredStaff[0];
            Assert.Equal("Surgeon", embolectomyStaff1.Specialization);
            Assert.Equal(2, embolectomyStaff1.Total);

            var embolectomyStaff2 = returnValue[1].RequiredStaff[1];
            Assert.Equal("Cardio", embolectomyStaff2.Specialization);
            Assert.Equal(3, embolectomyStaff2.Total);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenOperationTypeDoesNotExist()
        {
            var operationTypeId = Guid.NewGuid();

            _mockRepo.Setup(repo => repo.GetByIdWithDetailsAsync(It.IsAny<OperationTypeId>()))
                     .ReturnsAsync((OperationType)null);

            var result = await _controller.GetGetById(operationTypeId);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithEmptyList_WhenNoOperationTypesExist()
        {
            var emptyList = new List<OperationType>();
            _mockRepo.Setup(repo => repo.GetAllWithDetailsAsync())
                     .ReturnsAsync(emptyList);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<ActionResult<IEnumerable<OperationTypeDto>>>(result);
            var returnValue = Assert.IsType<List<OperationTypeDto>>(okResult.Value);

            Assert.Empty(returnValue);
        }
    }
}
