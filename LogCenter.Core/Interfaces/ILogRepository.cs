using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LogCenter.Core.Models;

namespace LogCenter.Core.Interfaces
{
    public interface ILogRepository
    {
        Task<LogEntry> GetByIdAsync(int id);
        Task<IEnumerable<LogEntry>> GetAllAsync();
        Task<IEnumerable<LogEntry>> GetByApplicationAsync(string application);
        Task<IEnumerable<LogEntry>> GetByLevelAsync(string level);
        Task<IEnumerable<LogEntry>> GetByDateRangeAsync(DateTime start, DateTime end);
        Task<IEnumerable<LogEntry>> SearchAsync(string searchTerm);
        Task<int> AddAsync(LogEntry logEntry);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<string>> GetDistinctApplicationsAsync();
        Task<IEnumerable<string>> GetDistinctEnvironmentsAsync();
        Task<IEnumerable<string>> GetDistinctLevelsAsync();
    }
} 