using System;

namespace LogCenter.Client
{
    public class LogCenterClientOptions
    {
        /// <summary>
        /// Gets or sets the base URL of the Log Center API.
        /// </summary>
        public Uri ApiBaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the name of the application sending logs.
        /// This is a required field.
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Gets or sets the environment name (e.g., "Production", "Development").
        /// Defaults to "Production" if not specified.
        /// </summary>
        public string Environment { get; set; } = "Production";

        /// <summary>
        /// Gets or sets the buffer size before logs are flushed to the server.
        /// Defaults to 100.
        /// </summary>
        public int BufferSize { get; set; } = 100;

        /// <summary>
        /// Gets or sets the interval for automatic buffer flushing.
        /// Set to TimeSpan.Zero to disable automatic flushing.
        /// Defaults to 10 seconds.
        /// </summary>
        public TimeSpan AutoFlushInterval { get; set; } = TimeSpan.FromSeconds(10);
    }
} 