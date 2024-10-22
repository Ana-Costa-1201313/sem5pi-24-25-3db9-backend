using System;
using System.Collections.Generic;
using Backoffice.Domain.OperationTypes;
using Backoffice.Domain.Specializations;
using Backoffice.Domain.Shared;
using Moq;
using Xunit;

namespace Backoffice.Tests
{
    public class OperationTypeTests
    {

        [Fact]
        public void TestOperationTypeWithValidInputs()
        {
            var name = new OperationTypeName("Appendectomy");
            var duration = new OperationTypeDuration(10, 60, 20);
            var requiredStaff = new List<OperationTypeRequiredStaff>
            {
                new OperationTypeRequiredStaff(new Specialization(new SpecializationName("Surgeon")), 2),
                new OperationTypeRequiredStaff(new Specialization(new SpecializationName("Cardio")), 3)
            };

            var operationType = new OperationType(name, duration, requiredStaff);

            Assert.Equal(name, operationType.Name);
            Assert.Equal(duration, operationType.Duration);
            Assert.Equal(requiredStaff, operationType.RequiredStaff);
            Assert.True(operationType.Active);
            Assert.NotNull(operationType.Id);
        }

        [Fact]
        public void TestOperationTypeWithNullName()
        {
            var duration = new OperationTypeDuration(10, 60, 20);
            var requiredStaff = new List<OperationTypeRequiredStaff>
            {
                new OperationTypeRequiredStaff(new Specialization(new SpecializationName("Surgeon")), 2)
            };

            var exception = Assert.Throws<BusinessRuleValidationException>(() =>
                new OperationType(null, duration, requiredStaff));
            Assert.Equal("Error: The operation type name can't be null.", exception.Message);
        }

        [Fact]
        public void TestOperationTypeWithEmptyName()
        {
            var duration = new OperationTypeDuration(10, 60, 20);
            var requiredStaff = new List<OperationTypeRequiredStaff>
            {
                new OperationTypeRequiredStaff(new Specialization(new SpecializationName("Surgeon")), 2)
            };

            var exception = Assert.Throws<BusinessRuleValidationException>(() =>
                new OperationType(new OperationTypeName(""), duration, requiredStaff));
            Assert.Equal("Error: The operation type name can't be null, empty or consist in only white spaces.", exception.Message);
        }

        [Fact]
        public void TestOperationTypeWithNullDuration()
        {
            var name = new OperationTypeName("Appendectomy");
            var requiredStaff = new List<OperationTypeRequiredStaff>
            {
                new OperationTypeRequiredStaff(new Specialization(new SpecializationName("Surgeon")), 2)
            };

            var exception = Assert.Throws<BusinessRuleValidationException>(() =>
                new OperationType(name, null, requiredStaff));
            Assert.Equal("Error: The operation type duration can't be null.", exception.Message);
        }

        [Fact]
        public void TestOperationTypeWithEmptyRequiredStaff()
        {
            var name = new OperationTypeName("Appendectomy");
            var duration = new OperationTypeDuration(10, 60, 20);

            var exception = Assert.Throws<BusinessRuleValidationException>(() =>
                new OperationType(name, duration, new List<OperationTypeRequiredStaff>()));

            Assert.Equal("Error: The operation type must have at least one required staff.", exception.Message);
        }

        [Fact]
        public void TestOperationTypeWithSingleValidRequiredStaff()
        {
            var name = new OperationTypeName("Appendectomy");
            var duration = new OperationTypeDuration(10, 60, 20);
            var requiredStaff = new List<OperationTypeRequiredStaff>
            {
                new OperationTypeRequiredStaff(new Specialization(new SpecializationName("Surgeon")), 2)
            };

            var operationType = new OperationType(name, duration, requiredStaff);

            Assert.Equal(name, operationType.Name);
            Assert.Equal(duration, operationType.Duration);
            Assert.Single(operationType.RequiredStaff);
        }

        //After teoric class

        [Fact]
        public void WhenPassingNameDurationAndRequiredStaff_ThenOperationTypeIsInstantiated()
        {

            Mock<OperationTypeName> opNameDouble = new Mock<OperationTypeName>();
            Mock<OperationTypeDuration> opDurationDouble = new Mock<OperationTypeDuration>();
            var opReqStaffMock = new Mock<OperationTypeRequiredStaff>();
            var opReqList = new List<OperationTypeRequiredStaff>
            {
                opReqStaffMock.Object
            };

            new OperationType(opNameDouble.Object, opDurationDouble.Object, opReqList);
        }

        [Fact]
        public void WhenPassingNullAsOperationTypeName_ThenThrowsException()
        {
            Mock<OperationTypeDuration> opDurationDouble = new Mock<OperationTypeDuration>();
            Mock<List<OperationTypeRequiredStaff>> opReqDouble = new Mock<List<OperationTypeRequiredStaff>>();

            var ex = Assert.Throws<BusinessRuleValidationException>(() => new OperationType(null, opDurationDouble.Object, opReqDouble.Object));
            Assert.Equal("Error: The operation type name can't be null.", ex.Message);
        }

        [Fact]
        public void WhenPassingNullAsOperationTypeDuration_ThenThrowsException()
        {
            Mock<OperationTypeName> opNameDouble = new Mock<OperationTypeName>();
            Mock<List<OperationTypeRequiredStaff>> opReqDouble = new Mock<List<OperationTypeRequiredStaff>>();

            var ex = Assert.Throws<BusinessRuleValidationException>(() => new OperationType(opNameDouble.Object, null, opReqDouble.Object));
            Assert.Equal("Error: The operation type duration can't be null.", ex.Message);
        }

        [Fact]
        public void WhenPassingNullAsOperationTypeRequiredStaff_ThenThrowsException()
        {
            Mock<OperationTypeName> opNameDouble = new Mock<OperationTypeName>();
            Mock<OperationTypeDuration> opDurationDouble = new Mock<OperationTypeDuration>();

            var ex = Assert.Throws<BusinessRuleValidationException>(() => new OperationType(opNameDouble.Object, opDurationDouble.Object, null));
            Assert.Equal("Error: The operation type can't have null required staff.", ex.Message);
        }

        [Fact]
        public void WhenPassingEmptyAsOperationTypeRequiredStaff_ThenThrowsException()
        {
            Mock<OperationTypeName> opNameDouble = new Mock<OperationTypeName>();
            Mock<OperationTypeDuration> opDurationDouble = new Mock<OperationTypeDuration>();
            var opReqList = new List<OperationTypeRequiredStaff>();

            var ex = Assert.Throws<BusinessRuleValidationException>(() => new OperationType(opNameDouble.Object, opDurationDouble.Object, opReqList));
            Assert.Equal("Error: The operation type must have at least one required staff.", ex.Message);
        }

        [Fact]
        public void WhenRequestingOperationTypeName_ThenReturnOperationTypeName()
        {
            string expectedName = "OpTypeA";

            var opMock = new Mock<OperationTypeName>();

            opMock.Setup(s => s.Name).Returns(expectedName);

            string actualName = opMock.Object.Name;

            Assert.Equal(expectedName, actualName);
        }

        [Fact]
        public void WhenRequestingOperationTypeDuration_ThenReturnOperationTypeDuration()
        {
            TimeSpan phase1 = TimeSpan.FromMinutes(20);
            TimeSpan phase2 = TimeSpan.FromMinutes(60);
            TimeSpan phase3 = TimeSpan.FromMinutes(30);

            var opMock = new Mock<OperationTypeDuration>();

            opMock.Setup(s => s.AnesthesiaPatientPreparationInMinutes).Returns(phase1);
            opMock.Setup(s => s.SurgeryInMinutes).Returns(phase2);
            opMock.Setup(s => s.CleaningInMinutes).Returns(phase3);

            TimeSpan actualPhase1 = opMock.Object.AnesthesiaPatientPreparationInMinutes;
            TimeSpan actualPhase2 = opMock.Object.SurgeryInMinutes;
            TimeSpan actualPhase3 = opMock.Object.CleaningInMinutes;
            

            Assert.Equal(phase1, actualPhase1);
            Assert.Equal(phase2, actualPhase2);
            Assert.Equal(phase3, actualPhase3);
        }


    }
}
