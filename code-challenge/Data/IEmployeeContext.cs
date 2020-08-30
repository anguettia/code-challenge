using challenge.Models;
using System.Threading.Tasks;

namespace challenge.Data
{
    public interface IEmployeeContext
    {
        Compensation GetByEmployeeId(string employeeId);
        void Add(Compensation compensation);
        Task SaveAsync();
    }
}