using challenge.Models;
using code_challenge.Tests.Integration.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;

namespace code_challenge.Tests.Integration
{
    [TestClass]
    public class CompensationControllerTests
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
        public void CreateCompensation_ValidCompensation_ReturnsCreated()
        {
            // Arrange
            var newCompensation = new Compensation
            {
                EmployeeId = "b7839309-3348-463b-a7e3-5de1c168beb3",
                EffectiveDate = DateTime.Now.Date,
                Salary = 100000
            };

            var expectedCompensation = new Compensation
            {
                Employee = new Employee
                {
                    EmployeeId = "b7839309-3348-463b-a7e3-5de1c168beb3",
                    FirstName = "Paul",
                    LastName = "McCartney"
                },
                EffectiveDate = DateTime.Now.Date,
                Salary = 100000
            };

            var requestContent = JsonConvert.SerializeObject(newCompensation);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/compensations",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var compensation = response.DeserializeContent<Compensation>();
            compensation.Employee.FirstName.Should().Be(expectedCompensation.Employee.FirstName);
            compensation.Employee.LastName.Should().Be(expectedCompensation.Employee.LastName);
            compensation.EffectiveDate.Should().Be(expectedCompensation.EffectiveDate);
            compensation.Salary.Should().Be(expectedCompensation.Salary);
        }

        [TestMethod]
        public void GetCompensationByEmployeeId_ValidEmployeeId_ReturnsOk()
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
                EffectiveDate = new DateTime(2020, 9, 1),
                Salary = 100000
            };

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/compensations/{expectedCompensation.Employee.EmployeeId}");
            var response = getRequestTask.Result;

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var compensation = response.DeserializeContent<Compensation>();
            compensation.Employee.FirstName.Should().Be(expectedCompensation.Employee.FirstName);
            compensation.Employee.LastName.Should().Be(expectedCompensation.Employee.LastName);
            compensation.EffectiveDate.Should().Be(expectedCompensation.EffectiveDate);
            compensation.Salary.Should().Be(expectedCompensation.Salary);
        }

        [TestMethod]
        public void GetCompensationByEmployeeId_NonExistent_ReturnsNotFound()
        {
            // Arrange
            const string employeeId = "non-existent-id";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/compensations/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var compensation = response.DeserializeContent<Compensation>();
            compensation.Should().BeNull();
        }
    }
}
