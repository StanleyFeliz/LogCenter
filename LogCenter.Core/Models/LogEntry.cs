using System;

namespace LogCenter.Core.Models
{
    public class LogEntry
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Level { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Exception { get; set; } = string.Empty;
        public string Application { get; set; } = string.Empty;
        public string Environment { get; set; } = string.Empty;
        public string MachineName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string StackTrace { get; set; } = string.Empty;
        public string AdditionalData { get; set; } = string.Empty;
    }
} 