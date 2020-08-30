using challenge.Models;
using code_challenge.Tests.Integration.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Http;

namespace code_challenge.Tests.Integration
{
    [TestClass]
    public class ReportingStructureControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<TestServerStartup>()
                .UseEnvironment("Development"));

            _httpClient = _testServer.CreateClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [TestMethod]
        public void GetReportingStructureById__ValidEmployeeId_ReturnsOk()
        {
            // Arrange
            const string employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            const string expectedFirstName = "John";
            const string expectedLastName = "Lennon";
            const int expectedNumberOfReports = 4;

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/reports/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var reportingStructure = response.DeserializeContent<ReportingStructure>();
            reportingStructure.Employee.FirstName.Should().Be(expectedFirstName);
            reportingStructure.Employee.LastName.Should().Be(expectedLastName);
            reportingStructure.NumberOfReports.Should().Be(expectedNumberOfReports);
        }

        [TestMethod]
        public void GetReportingStructureById_NonExistent_ReturnsNotFound()
        {
            // Arrange
            const string employeeId = "non-existent-id";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/reports/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var reportingStructure = response.DeserializeContent<ReportingStructure>();
            reportingStructure.Should().BeNull();
        }
    }
}
