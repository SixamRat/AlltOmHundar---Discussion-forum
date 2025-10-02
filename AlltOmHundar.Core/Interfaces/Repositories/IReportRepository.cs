using AlltOmHundar.Core.Models;
using AlltOmHundar.Core.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlltOmHundar.Core.Interfaces.Repositories
{
    public interface IReportRepository : IRepository<Report>
    {
        Task<IEnumerable<Report>> GetPendingReportsAsync();
        Task<IEnumerable<Report>> GetReportsByStatusAsync(ReportStatus status);
    }
}