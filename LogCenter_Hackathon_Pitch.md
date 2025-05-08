# LogCenter: Centralized Logging Made Simple

## The Problem

Managing logs across multiple applications is difficult:
- Logs scattered across different systems and formats
- No centralized view of application health
- Hard to spot patterns across services
- Debugging distributed systems is time-consuming
- Custom visualization for each application

## Our Solution

**LogCenter** is a powerful, centralized logging system for .NET applications that provides:

🔄 **Unified View**: All logs from every application in one dashboard  
🔍 **Powerful Search**: Quickly find relevant information  
📊 **Visual Analytics**: Charts and statistics to identify trends  
🚨 **Real-time Monitoring**: Instant visibility into issues  
⚡ **Simple Integration**: Just a few lines of code to connect apps  

## Technical Implementation

- **API** (.NET 8): RESTful endpoints for log ingestion and retrieval
- **Database**: Efficient storage with optimized queries and indexing
- **Dashboard** (Blazor WebAssembly): Modern, responsive UI for log analysis
- **Client Library**: Drop-in integration for any .NET application

```csharp
// Just add this to your application:
services.AddLogCenterClient(options => {
    options.ApiBaseUrl = new Uri("https://log-center-api");
    options.ApplicationName = "YourApp";
});

// Then log anywhere in your code:
await _logClient.LogInformationAsync("User logged in");
await _logClient.LogErrorAsync("Payment failed", exception);
```

## Key Features

- **Log Collection**: Buffer-based client for efficiency with automatic retries
- **Smart Filtering**: Group by application, level, timestamp, and more
- **Custom Analytics**: Charts showing error rates, application activity
- **Real-time Updates**: Live dashboard with auto-refresh capabilities
- **Multi-environment Support**: Dev, QA, Production isolation

## Why LogCenter?

Unlike existing solutions, LogCenter:
- **Zero Configuration**: Works out-of-the-box with smart defaults
- **Complete Control**: Host it yourself, own your data
- **Fast & Responsive**: Modern SPA with optimized back-end
- **Cross-Platform**: Works on any OS with .NET support
- **Minimal Overhead**: Efficient client with negligible performance impact

## Ready To Ship

- Complete solution with front-end and back-end components
- Client library ready for NuGet packaging
- Documentation and examples included
- Extensible architecture for future enhancements

**LogCenter** lets your team focus on building features instead of hunting through logs! 