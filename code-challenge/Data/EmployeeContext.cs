using challenge.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Data
{
    public class EmployeeContext : DbContext, IEmployeeContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options) : base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Compensation> Compensations { get; set; }

        public virtual void Add(Compensation compensation)
        {
            this.Compensations.Add(compensation);
        }

        public virtual Compensation GetByEmployeeId(string employeeId)
        {
            this.Employees.ToList();
            return this.Compensations.ToList().SingleOrDefault(c => c.EmployeeId == employeeId);
        }
        public virtual Task SaveAsync()
        {
            return this.SaveChangesAsync();
        }
    }
}
