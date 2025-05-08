using Asp.Versioning;
using LogCenter.Core.Interfaces;
using LogCenter.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace LogCenter.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class LogsController : ControllerBase
    {
        private readonly ILogService _logService;

        public LogsController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LogEntry>>> GetLogs()
        {
            var logs = await _logService.GetAllLogsAsync();
            return Ok(logs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LogEntry>> GetLog(int id)
        {
            var log = await _logService.GetLogByIdAsync(id);
            if (log == null)
            {
                return NotFound();
            }
            return Ok(log);
        }

        [HttpGet("application/{application}")]
        public async Task<ActionResult<IEnumerable<LogEntry>>> GetLogsByApplication(string application)
        {
            var logs = await _logService.GetLogsByApplicationAsync(application);
            return Ok(logs);
        }

        [HttpGet("level/{level}")]
        public async Task<ActionResult<IEnumerable<LogEntry>>> GetLogsByLevel(string level)
        {
            var logs = await _logService.GetLogsByLevelAsync(level);
            return Ok(logs);
        }

        [HttpGet("daterange")]
        public async Task<ActionResult<IEnumerable<LogEntry>>> GetLogsByDateRange(
            [FromQuery] DateTime start, 
            [FromQuery] DateTime end)
        {
            var logs = await _logService.GetLogsByDateRangeAsync(start, end);
            return Ok(logs);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<LogEntry>>> SearchLogs([FromQuery] string term)
        {
            var logs = await _logService.SearchLogsAsync(term);
            return Ok(logs);
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateLog(LogEntry logEntry)
        {
            var id = await _logService.CreateLogAsync(logEntry);
            return CreatedAtAction(nameof(GetLog), new { id }, id);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteLog(int id)
        {
            var result = await _logService.DeleteLogAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("stats/applications")]
        public async Task<ActionResult<Dictionary<string, int>>> GetLogCountByApplication()
        {
            var stats = await _logService.GetLogCountByApplicationAsync();
            return Ok(stats);
        }

        [HttpGet("stats/levels")]
        public async Task<ActionResult<Dictionary<string, int>>> GetLogCountByLevel()
        {
            var stats = await _logService.GetLogCountByLevelAsync();
            return Ok(stats);
        }

        [HttpGet("stats/dates")]
        public async Task<ActionResult<Dictionary<DateTime, int>>> GetLogCountByDate(
            [FromQuery] DateTime start, 
            [FromQuery] DateTime end)
        {
            var stats = await _logService.GetLogCountByDateAsync(start, end);
            return Ok(stats);
        }

        [HttpGet("distinct/applications")]
        public async Task<ActionResult<IEnumerable<string>>> GetDistinctApplications()
        {
            var applications = await _logService.GetDistinctApplicationsAsync();
            return Ok(applications);
        }

        [HttpGet("distinct/environments")]
        public async Task<ActionResult<IEnumerable<string>>> GetDistinctEnvironments()
        {
            var environments = await _logService.GetDistinctEnvironmentsAsync();
            return Ok(environments);
        }

        [HttpGet("distinct/levels")]
        public async Task<ActionResult<IEnumerable<string>>> GetDistinctLevels()
        {
            var levels = await _logService.GetDistinctLevelsAsync();
            return Ok(levels);
        }
    }
}
