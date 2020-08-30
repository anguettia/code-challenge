using challenge.Models;
using System;

namespace challenge.Services
{
    public interface ICompensationService
    {
        Compensation GetByEmployeeId(String id);
        Compensation Create(Compensation compensation);
    }
}
