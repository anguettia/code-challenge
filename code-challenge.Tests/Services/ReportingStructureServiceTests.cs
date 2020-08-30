using challenge.Models;
using challenge.Repositories;
using challenge.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace code_challenge.Tests.Integration.Services
{
    [TestClass]
    public class ReportingStructureServiceTests
    {
        private static Employee _expectedEmployee;

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            _expectedEmployee = new Employee
            {
                EmployeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f",
                FirstName = "John",
                LastName = "Lennon",
                DirectReports = new List<Employee>{
                    new Employee
                    {
                        EmployeeId = "b7839309-3348-463b-a7e3-5de1c168beb3",
                        FirstName = "Paul",
                        LastName = "McCartney",
                    },
                    new Employee
                    {
                        EmployeeId = "03aa1462-ffa9-4978-901b-7c001562cf6f",
                        DirectReports = new List<Employee>{
                            new Employee
                            {
                                EmployeeId = "62c1084e-6e34-4630-93fd-9153afb65309"
                            },
                            new Employee
                            {
                                EmployeeId = "c0c2293d-16bd-4603-8e08-638a9d18b22c"
                            }
                        }
                    }
                }
            };
        }

        [TestMethod]
        public void GetById_ValidEmployeeId_ReturnsReportingStructure()
        {
            // Arrange
            const int expectedNumberOfReports = 4;
            var logger = new Mock<ILogger<IReportingStructureService>>();
            var employeeRepository = new Mock<IEmployeeRepository>();

            employeeRepository.Setup(repository => repository.GetById(_expectedEmployee.EmployeeId))
                .Returns(_expectedEmployee);

            // Execute
            var reportingStructure = new ReportingStructureService(logger.Object, employeeRepository.Object).GetById(_expectedEmployee.EmployeeId);

            // Assert
            reportingStructure.Should().NotBeNull();
            reportingStructure.Employee.FirstName.Should().Be(_expectedEmployee.FirstName);
            reportingStructure.Employee.LastName.Should().Be(_expectedEmployee.LastName);
            reportingStructure.NumberOfReports.Should().Be(expectedNumberOfReports);
        }

        [TestMethod]
        public void GetById_WhitespaceEmployeeId_ReturnsNull()
        {
            // Arrange
            var logger = new Mock<ILogger<IReportingStructureService>>();
            var employeeRepository = new Mock<IEmployeeRepository>();

            // Execute
            var reportingStructure = new ReportingStructureService(logger.Object, employeeRepository.Object).GetById(" ");

            // Assert
            reportingStructure.Should().BeNull();
        }

        [TestMethod]
        public void GetById_EmptyEmployeeId_ReturnsNull()
        {
            // Arrange
            var logger = new Mock<ILogger<IReportingStructureService>>();
            var employeeRepository = new Mock<IEmployeeRepository>();

            // Execute
            var reportingStructure = new ReportingStructureService(logger.Object, employeeRepository.Object).GetById(string.Empty);

            // Assert
            reportingStructure.Should().BeNull();
        }

        [TestMethod]
        public void GetById_NullEmployeeId_ReturnsNull()
        {
            // Arrange
            var logger = new Mock<ILogger<IReportingStructureService>>();
            var employeeRepository = new Mock<IEmployeeRepository>();

            // Execute
            var reportingStructure = new ReportingStructureService(logger.Object, employeeRepository.Object).GetById(null);

            // Assert
            reportingStructure.Should().BeNull();
        }
    }
}
