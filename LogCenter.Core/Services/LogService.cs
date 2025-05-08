using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogCenter.Core.Interfaces;
using LogCenter.Core.Models;

namespace LogCenter.Core.Services
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepository;

        public LogService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task<LogEntry> GetLogByIdAsync(int id)
        {
            return await _logRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<LogEntry>> GetAllLogsAsync()
        {
            return await _logRepository.GetAllAsync();
        }

        public async Task<IEnumerable<LogEntry>> GetLogsByApplicationAsync(string application)
        {
            return await _logRepository.GetByApplicationAsync(application);
        }

        public async Task<IEnumerable<LogEntry>> GetLogsByLevelAsync(string level)
        {
            return await _logRepository.GetByLevelAsync(level);
        }

        public async Task<IEnumerable<LogEntry>> GetLogsByDateRangeAsync(DateTime start, DateTime end)
        {
            return await _logRepository.GetByDateRangeAsync(start, end);
        }

        public async Task<IEnumerable<LogEntry>> SearchLogsAsync(string searchTerm)
        {
            return await _logRepository.SearchAsync(searchTerm);
        }

        public async Task<int> CreateLogAsync(LogEntry logEntry)
        {
            // Set timestamp to now if not provided
            if (logEntry.Timestamp == default)
            {
                logEntry.Timestamp = DateTime.UtcNow;
            }

            return await _logRepository.AddAsync(logEntry);
        }

        public async Task<bool> DeleteLogAsync(int id)
        {
            return await _logRepository.DeleteAsync(id);
        }

        public async Task<Dictionary<string, int>> GetLogCountByApplicationAsync()
        {
            var logs = await _logRepository.GetAllAsync();
            return logs.GroupBy(l => l.Application)
                      .ToDictionary(g => g.Key, g => g.Count());
        }

        public async Task<Dictionary<string, int>> GetLogCountByLevelAsync()
        {
            var logs = await _logRepository.GetAllAsync();
            return logs.GroupBy(l => l.Level)
                      .ToDictionary(g => g.Key, g => g.Count());
        }

        public async Task<Dictionary<DateTime, int>> GetLogCountByDateAsync(DateTime start, DateTime end)
        {
            var logs = await _logRepository.GetByDateRangeAsync(start, end);
            return logs.GroupBy(l => l.Timestamp.Date)
                      .ToDictionary(g => g.Key, g => g.Count());
        }

        public async Task<IEnumerable<string>> GetDistinctApplicationsAsync()
        {
            return await _logRepository.GetDistinctApplicationsAsync();
        }

        public async Task<IEnumerable<string>> GetDistinctEnvironmentsAsync()
        {
            return await _logRepository.GetDistinctEnvironmentsAsync();
        }

        public async Task<IEnumerable<string>> GetDistinctLevelsAsync()
        {
            return await _logRepository.GetDistinctLevelsAsync();
        }
    }
} 