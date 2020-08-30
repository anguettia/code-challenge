using challenge.Data;
using challenge.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace challenge.Repositories
{
    public class CompensationRepository : ICompensationRepository
    {
        private readonly IEmployeeContext _compensationContext;
        private readonly ILogger<ICompensationRepository> _logger;

        public CompensationRepository(ILogger<ICompensationRepository> logger, IEmployeeContext compensationContext)
        {
            _compensationContext = compensationContext;
            _logger = logger;
        }

        public Compensation Add(Compensation compensation)
        {
            if (compensation == null || string.IsNullOrWhiteSpace(compensation.EmployeeId))
                return null;

            compensation.CompensationId = Guid.NewGuid().ToString();
            _compensationContext.Add(compensation);

            return compensation;
        }

        public Compensation GetByEmployeeId(string employeeId)
        {
            return _compensationContext.GetByEmployeeId(employeeId);
        }

        public Task SaveAsync()
        {
            return _compensationContext.SaveAsync();
        }
    }
}
