using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using HealthcareApp.Controllers;
using HealthcareApp.Domain.OperationTypes;
using HealthcareApp.Domain.Shared;
using HealthcareApp.Domain.Specializations;
using Microsoft.AspNetCore.Mvc;
using Xunit.Abstractions;

namespace HealthcareApp.Tests.Controllers
{
    public class SpecializationControllerTests
    {
        private readonly Mock<ISpecializationRepository> _mockRepo;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly SpecializationsController _controller;
        private readonly SpecializationService _service;

        public SpecializationControllerTests()
        {
            _mockRepo = new Mock<ISpecializationRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _service = new SpecializationService(
                _mockUnitOfWork.Object,
                _mockRepo.Object
            );
            _controller = new SpecializationsController(_service);
        }



        [Fact]
        public async Task GetById_ReturnsOkResult_WithSpecialization()
        {

            var spec = new Specialization(new SpecializationName("Surgeon"));


            _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<SpecializationId>()))
                     .ReturnsAsync(spec);

            var result = await _controller.GetGetById(spec.Id.AsGuid());

            var okResult = Assert.IsType<ActionResult<SpecializationDto>>(result);
            var returnValue = Assert.IsType<SpecializationDto>(okResult.Value);

            Assert.Equal("Surgeon", returnValue.Name);
        }


        [Fact]
        public async Task GetAll_ReturnsOkResult_WithSpecializations()
        {
            var spec = new Specialization(new SpecializationName("Surgeon"));
            var spec2 = new Specialization(new SpecializationName("Cardio"));




            var listSpec = new List<Specialization> { spec, spec2 };

            _mockRepo.Setup(repo => repo.GetAllAsync())
                     .ReturnsAsync(listSpec);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<ActionResult<IEnumerable<SpecializationDto>>>(result);
            var returnValue = Assert.IsType<List<SpecializationDto>>(okResult.Value);

            Assert.Equal(2, returnValue.Count);
            Assert.Equal("Surgeon", returnValue[0].Name);
            Assert.Equal("Cardio", returnValue[1].Name);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenSpecializationNotExist()
        {
            var specId = Guid.NewGuid();

            _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<SpecializationId>()))
                     .ReturnsAsync((Specialization)null);

            var result = await _controller.GetGetById(specId);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithEmptyList_WhenNoSpecializationExist()
        {
            var emptyList = new List<Specialization>();
            _mockRepo.Setup(repo => repo.GetAllAsync())
                     .ReturnsAsync(emptyList);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<ActionResult<IEnumerable<SpecializationDto>>>(result);
            var returnValue = Assert.IsType<List<SpecializationDto>>(okResult.Value);

            Assert.Empty(returnValue);
        }
    }
}
