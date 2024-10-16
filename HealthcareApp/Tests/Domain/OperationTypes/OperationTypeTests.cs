using System;
using System.Collections.Generic;
using HealthcareApp.Domain.OperationTypes;
using HealthcareApp.Domain.Specializations;
using HealthcareApp.Domain.Shared;
using Moq;
using Xunit;

namespace HealthcareApp.Tests
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

        
    }
}
