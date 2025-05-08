using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogCenter.Core.Interfaces;
using LogCenter.Core.Models;
using LogCenter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LogCenter.Infrastructure.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly LogDbContext _context;

        public LogRepository(LogDbContext context)
        {
            _context = context;
        }

        public async Task<LogEntry> GetByIdAsync(int id)
        {
            return await _context.LogEntries.FindAsync(id);
        }

        public async Task<IEnumerable<LogEntry>> GetAllAsync()
        {
            return await _context.LogEntries
                .OrderByDescending(l => l.Timestamp)
                .ToListAsync();
        }

        public async Task<IEnumerable<LogEntry>> GetByApplicationAsync(string application)
        {
            return await _context.LogEntries
                .Where(l => l.Application == application)
                .OrderByDescending(l => l.Timestamp)
                .ToListAsync();
        }

        public async Task<IEnumerable<LogEntry>> GetByLevelAsync(string level)
        {
            return await _context.LogEntries
                .Where(l => l.Level == level)
                .OrderByDescending(l => l.Timestamp)
                .ToListAsync();
        }

        public async Task<IEnumerable<LogEntry>> GetByDateRangeAsync(DateTime start, DateTime end)
        {
            return await _context.LogEntries
                .Where(l => l.Timestamp >= start && l.Timestamp <= end)
                .OrderByDescending(l => l.Timestamp)
                .ToListAsync();
        }

        public async Task<IEnumerable<LogEntry>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync();

            return await _context.LogEntries
                .Where(l => l.Message.Contains(searchTerm) || 
                           l.Exception.Contains(searchTerm) ||
                           l.StackTrace.Contains(searchTerm))
                .OrderByDescending(l => l.Timestamp)
                .ToListAsync();
        }

        public async Task<int> AddAsync(LogEntry logEntry)
        {
            _context.LogEntries.Add(logEntry);
            await _context.SaveChangesAsync();
            return logEntry.Id;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var logEntry = await _context.LogEntries.FindAsync(id);
            if (logEntry == null) return false;
            
            _context.LogEntries.Remove(logEntry);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<string>> GetDistinctApplicationsAsync()
        {
            return await _context.LogEntries
                .Select(l => l.Application)
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetDistinctEnvironmentsAsync()
        {
            return await _context.LogEntries
                .Select(l => l.Environment)
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetDistinctLevelsAsync()
        {
            return await _context.LogEntries
                .Select(l => l.Level)
                .Distinct()
                .ToListAsync();
        }
    }
} 