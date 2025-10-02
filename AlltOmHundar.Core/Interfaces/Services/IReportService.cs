using AlltOmHundar.Core.Models;
using AlltOmHundar.Core.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlltOmHundar.Core.Interfaces.Services
{
    public interface IReportService
    {
        Task<Report> CreateReportAsync(int postId, int reportedByUserId, string reason);
        Task<IEnumerable<Report>> GetPendingReportsAsync();
        Task<IEnumerable<Report>> GetReportsByStatusAsync(ReportStatus status);
        Task<bool> UpdateReportStatusAsync(int reportId, ReportStatus status, int reviewedByUserId, string? adminNotes = null);
    }
}