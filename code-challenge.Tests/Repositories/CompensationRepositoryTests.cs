using challenge.Data;
using challenge.Models;
using challenge.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace code_challenge.Tests.Integration.Repositories
{
    [TestClass]
    public class CompensationRepositoryTests
    {
        [TestMethod]
        public void Add_ValidCompensation_ReturnsCompensation()
        {
            // Arrange
            var expectedCompensation = new Compensation
            {
                EmployeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f",
                EffectiveDate = DateTime.Now.Date,
                Salary = 100000
            };

            Compensation persistedCompensation = null;
            var logger = new Mock<ILogger<ICompensationRepository>>();
            var context = new Mock<IEmployeeContext>();

            context.Setup(c => c.Add(It.IsAny<Compensation>()))
                .Callback((Compensation data) => persistedCompensation = data);

            context.Setup(c => c.GetByEmployeeId(expectedCompensation.EmployeeId))
                .Returns(expectedCompensation);

            // Execute
            var compensation = new CompensationRepository(logger.Object, context.Object).Add(expectedCompensation);

            // Assert
            persistedCompensation.Should().NotBeNull();
            persistedCompensation.EmployeeId.Should().Be(expectedCompensation.EmployeeId);
            persistedCompensation.EffectiveDate.Should().Be(expectedCompensation.EffectiveDate);
            persistedCompensation.Salary.Should().Be(expectedCompensation.Salary);

            compensation.Should().NotBeNull();
            compensation.EmployeeId.Should().Be(expectedCompensation.EmployeeId);
            compensation.EffectiveDate.Should().Be(expectedCompensation.EffectiveDate);
            compensation.Salary.Should().Be(expectedCompensation.Salary);
        }

        [TestMethod]
        public void Add_NullCompensation_ReturnsNull()
        {
            // Arrange
            var logger = new Mock<ILogger<ICompensationRepository>>();
            var context = new Mock<IEmployeeContext>();

            // Execute
            var compensation = new CompensationRepository(logger.Object, context.Object).Add(compensation: null);

            // Assert
            compensation.Should().BeNull();
        }

        [TestMethod]
        public void Add_WhitespaceEmployeeId_ReturnsNull()
        {
            // Arrange
            var expectedCompensation = new Compensation
            {
                EmployeeId = " ",
                EffectiveDate = DateTime.Now.Date,
                Salary = 100000
            };
            var logger = new Mock<ILogger<ICompensationRepository>>();
            var context = new Mock<IEmployeeContext>();

            // Execute
            var compensation = new CompensationRepository(logger.Object, context.Object).Add(expectedCompensation);

            // Assert
            compensation.Should().BeNull();
        }

        [TestMethod]
        public void Add_EmptyEmployeeId_ReturnsNull()
        {
            // Arrange
            var expectedCompensation = new Compensation
            {
                EmployeeId = string.Empty,
                EffectiveDate = DateTime.Now.Date,
                Salary = 100000
            };
            var logger = new Mock<ILogger<ICompensationRepository>>();
            var context = new Mock<IEmployeeContext>();

            // Execute
            var compensation = new CompensationRepository(logger.Object, context.Object).Add(expectedCompensation);

            // Assert
            compensation.Should().BeNull();
        }

        [TestMethod]
        public void Add_NullEmployeeId_ReturnsNull()
        {
            // Arrange
            var expectedCompensation = new Compensation
            {
                EmployeeId = null,
                EffectiveDate = DateTime.Now.Date,
                Salary = 100000
            };
            var logger = new Mock<ILogger<ICompensationRepository>>();
            var context = new Mock<IEmployeeContext>();

            // Execute
            var compensation = new CompensationRepository(logger.Object, context.Object).Add(expectedCompensation);

            // Assert
            compensation.Should().BeNull();
        }

        [TestMethod]
        public void GetByEmployeeId_ValidEmployeeId_ReturnsCompensation()
        {
            // Arrange
            var expectedCompensation = new Compensation
            {
                Employee = new Employee
                {
                    EmployeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f",
                    FirstName = "John",
                    LastName = "Lennon"
                },
                EffectiveDate = DateTime.Now.Date,
                Salary = 100000
            };
            var logger = new Mock<ILogger<ICompensationRepository>>();
            var context = new Mock<IEmployeeContext>();

            context.Setup(c => c.GetByEmployeeId(expectedCompensation.Employee.EmployeeId))
                .Returns(expectedCompensation);

            // Execute
            var compensation = new CompensationRepository(logger.Object, context.Object).GetByEmployeeId(expectedCompensation.Employee.EmployeeId);

            // Assert
            compensation.Should().NotBeNull();
            compensation.Employee.Should().NotBeNull();
            compensation.Employee.FirstName.Should().Be(expectedCompensation.Employee.FirstName);
            compensation.Employee.LastName.Should().Be(expectedCompensation.Employee.LastName);
            compensation.EffectiveDate.Should().Be(expectedCompensation.EffectiveDate);
            compensation.Salary.Should().Be(expectedCompensation.Salary);
        }

        [TestMethod]
        public void GetByEmployeeId_NonExistentEmployeeId_ReturnsNull()
        {
            // Arrange
            var logger = new Mock<ILogger<ICompensationRepository>>();
            var context = new Mock<IEmployeeContext>();

            context.Setup(c => c.GetByEmployeeId(It.IsAny<string>()))
                .Returns(() => null);

            // Execute
            var compensation = new CompensationRepository(logger.Object, context.Object).GetByEmployeeId("non-existent-id");

            // Assert
            compensation.Should().BeNull();
        }

        [TestMethod]
        public void GetByEmployeeId_WhitespaceEmployeeId_ReturnsNull()
        {
            // Arrange
            var logger = new Mock<ILogger<ICompensationRepository>>();
            var context = new Mock<IEmployeeContext>();

            // Execute
            var compensation = new CompensationRepository(logger.Object, context.Object).GetByEmployeeId(employeeId: " ");

            // Assert
            compensation.Should().BeNull();
        }

        [TestMethod]
        public void GetByEmployeeId_EmptyEmployeeId_ReturnsNull()
        {
            // Arrange
            var logger = new Mock<ILogger<ICompensationRepository>>();
            var context = new Mock<IEmployeeContext>();

            // Execute
            var compensation = new CompensationRepository(logger.Object, context.Object).GetByEmployeeId(employeeId: string.Empty);

            // Assert
            compensation.Should().BeNull();
        }

        [TestMethod]
        public void GetByEmployeeId_NullEmployeeId_ReturnsNull()
        {
            // Arrange
            var logger = new Mock<ILogger<ICompensationRepository>>();
            var context = new Mock<IEmployeeContext>();

            // Execute
            var compensation = new CompensationRepository(logger.Object, context.Object).GetByEmployeeId(employeeId: null);

            // Assert
            compensation.Should().BeNull();
        }
    }
}
