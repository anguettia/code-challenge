using challenge.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Data
{
    public class CompensationDataSeeder
    {
        private EmployeeContext _employeeContext;
        private const String COMPENSATION_SEED_DATA_FILE = "resources/CompensationSeedData.json";

        public CompensationDataSeeder(EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
        }

        public async Task Seed()
        {
            if (_employeeContext.Compensations.Any())
                return;

            var compensations = LoadCompensations();
            _employeeContext.Compensations.AddRange(compensations);

            await _employeeContext.SaveChangesAsync();
        }

        private List<Compensation> LoadCompensations()
        {
            return JsonConvert.DeserializeObject<List<Compensation>>(File.ReadAllText(COMPENSATION_SEED_DATA_FILE));
        }
    }
}
