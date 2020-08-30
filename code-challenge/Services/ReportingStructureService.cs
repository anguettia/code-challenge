using challenge.Models;
using challenge.Repositories;
using Microsoft.Extensions.Logging;
using System;

namespace challenge.Services
{
    public class ReportingStructureService : IReportingStructureService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<IReportingStructureService> _logger;

        public ReportingStructureService(ILogger<IReportingStructureService> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public ReportingStructure GetById(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
                return null;

            var employee = _employeeRepository.GetById(id);

            return (employee == null)
                ? null
                : new ReportingStructure
                {
                    Employee = employee,
                    NumberOfReports = getNumberOfReports(employee)
                };
        }

        private int getNumberOfReports(Employee employee)
        {
            if (employee.DirectReports == null)
                return 0;

            var numberOfReports = employee.DirectReports.Count;

            employee.DirectReports.ForEach(directReport =>
                numberOfReports += getNumberOfReports(directReport)
            );

            return numberOfReports;
        }
    }
}
