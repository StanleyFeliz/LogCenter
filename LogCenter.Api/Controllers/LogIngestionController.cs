using Asp.Versioning;
using LogCenter.Core.Interfaces;
using LogCenter.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace LogCenter.Api.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class LogIngestionController : ControllerBase
    {
        private readonly ILogService _logService;
        private readonly ILogger<LogIngestionController> _logger;

        public LogIngestionController(ILogService logService, ILogger<LogIngestionController> logger)
        {
            _logService = logService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> IngestLog(LogEntry logEntry)
        {
            try
            {
                // Validate required fields
                if (string.IsNullOrWhiteSpace(logEntry.Message))
                {
                    return BadRequest("Log message is required");
                }

                if (string.IsNullOrWhiteSpace(logEntry.Application))
                {
                    return BadRequest("Application name is required");
                }

                if (string.IsNullOrWhiteSpace(logEntry.Level))
                {
                    return BadRequest("Log level is required");
                }

                // Set default values if not provided
                if (logEntry.Timestamp == default)
                {
                    logEntry.Timestamp = DateTime.UtcNow;
                }

                if (string.IsNullOrWhiteSpace(logEntry.Environment))
                {
                    logEntry.Environment = "Production"; // Default
                }

                if (string.IsNullOrWhiteSpace(logEntry.MachineName))
                {
                    logEntry.MachineName = Environment.MachineName;
                }

                // Save the log entry
                var id = await _logService.CreateLogAsync(logEntry);
                
                _logger.LogInformation("Ingested log from {Application} with level {Level}", 
                    logEntry.Application, logEntry.Level);

                return Ok(new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ingesting log entry");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpPost("batch")]
        public async Task<IActionResult> IngestBatchLogs(LogEntry[] logEntries)
        {
            try
            {
                if (logEntries == null || logEntries.Length == 0)
                {
                    return BadRequest("No log entries provided");
                }

                var results = new int[logEntries.Length];

                for (int i = 0; i < logEntries.Length; i++)
                {
                    var logEntry = logEntries[i];

                    // Set default values if not provided
                    if (logEntry.Timestamp == default)
                    {
                        logEntry.Timestamp = DateTime.UtcNow;
                    }

                    if (string.IsNullOrWhiteSpace(logEntry.Environment))
                    {
                        logEntry.Environment = "Production"; // Default
                    }

                    if (string.IsNullOrWhiteSpace(logEntry.MachineName))
                    {
                        logEntry.MachineName = Environment.MachineName;
                    }

                    // Save the log entry
                    results[i] = await _logService.CreateLogAsync(logEntry);
                }

                _logger.LogInformation("Ingested batch of {Count} log entries", logEntries.Length);

                return Ok(new { Count = logEntries.Length, Ids = results });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ingesting batch log entries");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }
    }
} 