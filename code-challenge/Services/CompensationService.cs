using challenge.Models;
using challenge.Repositories;
using Microsoft.Extensions.Logging;
using System;

namespace challenge.Services
{
    public class CompensationService : ICompensationService
    {
        private readonly ICompensationRepository _compensationRepository;
        private readonly ILogger<ICompensationService> _logger;

        public CompensationService(ILogger<ICompensationService> logger, ICompensationRepository compensationRepository)
        {
            _compensationRepository = compensationRepository;
            _logger = logger;
        }

        public Compensation Create(Compensation compensation)
        {
            if (compensation == null || string.IsNullOrWhiteSpace(compensation.EmployeeId))
                return null;

            _compensationRepository.Add(compensation);
            _compensationRepository.SaveAsync().Wait();

            return _compensationRepository.GetByEmployeeId(compensation.EmployeeId);
        }

        public Compensation GetByEmployeeId(string employeeId)
        {
            if (string.IsNullOrWhiteSpace(employeeId))
                return null;

            return _compensationRepository.GetByEmployeeId(employeeId);
        }
    }
}
