using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LogCenter.Core.Models;

namespace LogCenter.Core.Interfaces
{
    public interface ILogService
    {
        Task<LogEntry> GetLogByIdAsync(int id);
        Task<IEnumerable<LogEntry>> GetAllLogsAsync();
        Task<IEnumerable<LogEntry>> GetLogsByApplicationAsync(string application);
        Task<IEnumerable<LogEntry>> GetLogsByLevelAsync(string level);
        Task<IEnumerable<LogEntry>> GetLogsByDateRangeAsync(DateTime start, DateTime end);
        Task<IEnumerable<LogEntry>> SearchLogsAsync(string searchTerm);
        Task<int> CreateLogAsync(LogEntry logEntry);
        Task<bool> DeleteLogAsync(int id);
        Task<Dictionary<string, int>> GetLogCountByApplicationAsync();
        Task<Dictionary<string, int>> GetLogCountByLevelAsync();
        Task<Dictionary<DateTime, int>> GetLogCountByDateAsync(DateTime start, DateTime end);
        Task<IEnumerable<string>> GetDistinctApplicationsAsync();
        Task<IEnumerable<string>> GetDistinctEnvironmentsAsync();
        Task<IEnumerable<string>> GetDistinctLevelsAsync();
    }
} 