using AlltOmHundar.Core.Interfaces.Repositories;
using AlltOmHundar.Core.Interfaces.Services;
using AlltOmHundar.Core.Models;
using AlltOmHundar.Core.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlltOmHundar.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<Report> CreateReportAsync(int postId, int reportedByUserId, string reason)
        {
            var report = new Report
            {
                PostId = postId,
                ReportedByUserId = reportedByUserId,
                Reason = reason,
                Status = ReportStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await _reportRepository.AddAsync(report);
            return report;
        }

        public async Task<IEnumerable<Report>> GetPendingReportsAsync()
        {
            return await _reportRepository.GetPendingReportsAsync();
        }

        public async Task<IEnumerable<Report>> GetReportsByStatusAsync(ReportStatus status)
        {
            return await _reportRepository.GetReportsByStatusAsync(status);
        }

        public async Task<bool> UpdateReportStatusAsync(int reportId, ReportStatus status, int reviewedByUserId, string? adminNotes = null)
        {
            var report = await _reportRepository.GetByIdAsync(reportId);
            if (report == null)
                return false;

            report.Status = status;
            report.ReviewedByUserId = reviewedByUserId;
            report.ReviewedAt = DateTime.UtcNow;
            report.AdminNotes = adminNotes;

            await _reportRepository.UpdateAsync(report);
            return true;
        }
    }
}