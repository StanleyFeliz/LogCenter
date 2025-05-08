using System;
using Microsoft.Extensions.DependencyInjection;

namespace LogCenter.Client
{
    public static class LogCenterClientExtensions
    {
        /// <summary>
        /// Adds the LogCenterClient to the service collection for dependency injection.
        /// </summary>
        public static IServiceCollection AddLogCenterClient(
            this IServiceCollection services, 
            Action<LogCenterClientOptions> configureOptions)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }
            
            // Register options
            var options = new LogCenterClientOptions();
            configureOptions(options);
            
            services.AddSingleton(options);
            
            // Add HttpClient with proper configuration
            services.AddHttpClient<LogCenterClient>((serviceProvider, client) =>
            {
                if (options.ApiBaseUrl != null)
                {
                    client.BaseAddress = options.ApiBaseUrl;
                }
            });
            
            // Register LogCenterClient
            services.AddSingleton<LogCenterClient>();
            
            return services;
        }
    }
} 