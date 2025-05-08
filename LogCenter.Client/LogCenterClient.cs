using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using LogCenter.Core.Models;

namespace LogCenter.Client
{
    public class LogCenterClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _applicationName;
        private readonly string _environment;
        private readonly Queue<LogEntry> _logBuffer;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly Timer _flushTimer;
        private readonly int _bufferSize;

        public LogCenterClient(HttpClient httpClient, LogCenterClientOptions options)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            
            if (string.IsNullOrWhiteSpace(options.ApplicationName))
                throw new ArgumentException("Application name must be provided", nameof(options.ApplicationName));
            
            _applicationName = options.ApplicationName;
            _environment = options.Environment ?? "Production";
            _bufferSize = options.BufferSize > 0 ? options.BufferSize : 100;
            _logBuffer = new Queue<LogEntry>();
            
            // Set up a timer to flush the buffer periodically
            if (options.AutoFlushInterval > TimeSpan.Zero)
            {
                _flushTimer = new Timer(
                    async _ => await FlushAsync(), 
                    null, 
                    options.AutoFlushInterval, 
                    options.AutoFlushInterval);
            }
            
            // Configure base address if provided
            if (options.ApiBaseUrl != null && !string.IsNullOrWhiteSpace(options.ApiBaseUrl.ToString()))
            {
                _httpClient.BaseAddress = options.ApiBaseUrl;
            }
        }

        /// <summary>
        /// Logs a message at the specified level
        /// </summary>
        public async Task LogAsync(string level, string message, Exception exception = null)
        {
            var logEntry = CreateLogEntry(level, message, exception);
            await AddToBufferAsync(logEntry);
        }

        /// <summary>
        /// Logs a message at Information level
        /// </summary>
        public async Task LogInformationAsync(string message)
        {
            await LogAsync("Information", message);
        }

        /// <summary>
        /// Logs a message at Warning level
        /// </summary>
        public async Task LogWarningAsync(string message)
        {
            await LogAsync("Warning", message);
        }

        /// <summary>
        /// Logs a message at Error level
        /// </summary>
        public async Task LogErrorAsync(string message, Exception exception = null)
        {
            await LogAsync("Error", message, exception);
        }

        /// <summary>
        /// Logs a message at Debug level
        /// </summary>
        public async Task LogDebugAsync(string message)
        {
            await LogAsync("Debug", message);
        }

        /// <summary>
        /// Flushes the log buffer and sends logs to the server
        /// </summary>
        public async Task FlushAsync()
        {
            await _semaphore.WaitAsync();
            
            try
            {
                if (_logBuffer.Count == 0)
                {
                    return;
                }

                var logs = new List<LogEntry>();
                
                while (_logBuffer.Count > 0)
                {
                    logs.Add(_logBuffer.Dequeue());
                }

                if (logs.Count > 0)
                {
                    // Send logs to server
                    if (logs.Count == 1)
                    {
                        await _httpClient.PostAsJsonAsync("api/v1/logingestion", logs[0]);
                    }
                    else
                    {
                        await _httpClient.PostAsJsonAsync("api/v1/logingestion/batch", logs.ToArray());
                    }
                }
            }
            catch (Exception)
            {
                // Consider adding retry logic or logging the error
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private LogEntry CreateLogEntry(string level, string message, Exception exception = null)
        {
            return new LogEntry
            {
                Timestamp = DateTime.UtcNow,
                Level = level,
                Message = message,
                Exception = exception?.ToString() ?? string.Empty,
                Application = _applicationName,
                Environment = _environment,
                MachineName = Environment.MachineName,
                Category = string.Empty,
                StackTrace = exception?.StackTrace ?? string.Empty
            };
        }

        private async Task AddToBufferAsync(LogEntry logEntry)
        {
            await _semaphore.WaitAsync();
            
            try
            {
                _logBuffer.Enqueue(logEntry);
                
                // If buffer reaches the threshold, flush it
                if (_logBuffer.Count >= _bufferSize)
                {
                    await FlushAsync();
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public void Dispose()
        {
            _flushTimer?.Dispose();
            _semaphore?.Dispose();
        }
    }
} 