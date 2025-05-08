# LogCenter

LogCenter is a centralized logging system built with .NET 8 that allows you to collect, store, and analyze logs from all your applications in one place. It provides a web-based dashboard for monitoring and analyzing log data.

## Project Structure

The solution consists of the following projects:

- **LogCenter.Core**: Contains domain models, interfaces, and core business logic
- **LogCenter.Infrastructure**: Implements data access and external service integrations
- **LogCenter.Api**: REST API that receives and stores logs and provides endpoints for querying
- **LogCenter.Dashboard**: Blazor WebAssembly application providing the user interface for log analysis
- **LogCenter.Client**: Client library that can be used by other applications to send logs to LogCenter

## Features

- **Centralized Log Storage**: Collect logs from multiple applications in one database
- **Real-time Log Dashboard**: View and search logs in real-time
- **Statistical Analysis**: View charts and trends based on log data
- **Filtering and Searching**: Easily find relevant logs with powerful search capabilities
- **Multiple Log Levels**: Support for standard log levels (Error, Warning, Information, Debug, etc.)
- **Application Grouping**: Group logs by application, environment, or other properties
- **Client Library**: Easy integration with your existing .NET applications

## Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server (or LocalDB for development)

### Installation

1. Clone the repository
2. Navigate to the solution directory
3. Run the API:

```bash
cd LogCenter.Api
dotnet run
```

4. Run the Dashboard:

```bash
cd LogCenter.Dashboard
dotnet run
```

### Configuration

By default, the application uses LocalDB. To use a different database, update the connection string in `LogCenter.Api/appsettings.json`.

## Using the Client Library

To send logs from your application to LogCenter, add the LogCenter.Client NuGet package to your project:

```bash
dotnet add package LogCenter.Client
```

Then configure the client in your Program.cs or Startup.cs:

```csharp
services.AddLogCenterClient(options =>
{
    options.ApiBaseUrl = new Uri("https://your-logcenter-api-url");
    options.ApplicationName = "YourApplicationName";
    options.Environment = "Production"; // or Development, Staging, etc.
});
```

In your code, inject and use the client:

```csharp
public class YourService
{
    private readonly LogCenterClient _logClient;
    
    public YourService(LogCenterClient logClient)
    {
        _logClient = logClient;
    }
    
    public async Task DoSomething()
    {
        try
        {
            // Your code here
            await _logClient.LogInformationAsync("Operation completed successfully");
        }
        catch (Exception ex)
        {
            await _logClient.LogErrorAsync("Operation failed", ex);
            throw;
        }
    }
}
```

## License

This project is licensed under the MIT License - see the LICENSE file for details. 