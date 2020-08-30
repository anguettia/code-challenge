using challenge.Models;
using System;

namespace challenge.Services
{
    public interface IReportingStructureService
    {
        ReportingStructure GetById(String id);
    }
}
