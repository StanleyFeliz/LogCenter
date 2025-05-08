using System.Net.Http.Json;
using System.Text.Json;
using LogCenter.Core.Models;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("LogCenter API Test Client");
Console.WriteLine("=========================");
Console.WriteLine();

// Ask for the API URL
Console.WriteLine("Which API URL would you like to use?");
Console.WriteLine("1. https://localhost:7020 (default HTTPS)");
Console.WriteLine("2. http://localhost:5208 (default HTTP)");
Console.WriteLine("3. Custom URL");
Console.Write("Option (default 1): ");

string apiUrl = "https://localhost:7020";
string input = Console.ReadLine();

if (input == "2")
{
    apiUrl = "http://localhost:5208";
}
else if (input == "3")
{
    Console.Write("Enter API URL: ");
    string customUrl = Console.ReadLine();
    if (!string.IsNullOrEmpty(customUrl))
    {
        apiUrl = customUrl;
    }
}

Console.WriteLine($"Using API URL: {apiUrl}");
Console.WriteLine();

// Setup HTTP client with SSL certificate validation disabled for development
var handler = new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
};

var services = new ServiceCollection();
services.AddHttpClient("LogCenterApi", client =>
{
    client.BaseAddress = new Uri(apiUrl);
}).ConfigurePrimaryHttpMessageHandler(() => handler);

var serviceProvider = services.BuildServiceProvider();
var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
var client = httpClientFactory.CreateClient("LogCenterApi");

while (true)
{
    Console.WriteLine("Select an option:");
    Console.WriteLine("1. Send a test log");
    Console.WriteLine("2. Send multiple test logs");
    Console.WriteLine("3. Get all logs");
    Console.WriteLine("4. Get logs by application");
    Console.WriteLine("5. Get log statistics");
    Console.WriteLine("0. Exit");
    Console.Write("Option: ");

    if (!int.TryParse(Console.ReadLine(), out int option))
    {
        Console.WriteLine("Invalid option. Please try again.");
        continue;
    }

    try
    {
        switch (option)
        {
            case 0:
                return;

            case 1:
                await SendTestLogAsync(client);
                break;

            case 2:
                await SendMultipleTestLogsAsync(client);
                break;

            case 3:
                await GetAllLogsAsync(client);
                break;

            case 4:
                await GetLogsByApplicationAsync(client);
                break;

            case 5:
                await GetLogStatisticsAsync(client);
                break;

            default:
                Console.WriteLine("Invalid option. Please try again.");
                break;
        }
    }
    catch (HttpRequestException ex)
    {
        Console.WriteLine($"API Connection Error: {ex.Message}");
        if (ex.InnerException != null)
        {
            Console.WriteLine($"Inner error: {ex.InnerException.Message}");
        }
        Console.WriteLine("\nPossible solutions:");
        Console.WriteLine("1. Ensure the API is running");
        Console.WriteLine("2. Check if the API URL is correct");
        Console.WriteLine("3. Verify there are no firewall issues");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
        if (ex.InnerException != null)
        {
            Console.WriteLine($"Inner error: {ex.InnerException.Message}");
        }
    }

    Console.WriteLine();
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey(true);
    Console.Clear();
}

async Task SendTestLogAsync(HttpClient client)
{
    Console.WriteLine("Sending a test log...");

    var logEntry = CreateTestLogEntry();
    Console.WriteLine("Log entry to send:");
    Console.WriteLine($"  Message: {logEntry.Message}");
    Console.WriteLine($"  Level: {logEntry.Level}");
    Console.WriteLine($"  Application: {logEntry.Application}");
    Console.WriteLine($"  Environment: {logEntry.Environment}");
    
    // Print the JSON that will be sent
    var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
    string jsonPayload = JsonSerializer.Serialize(logEntry, jsonOptions);
    Console.WriteLine("JSON payload:");
    Console.WriteLine(jsonPayload);
    
    var response = await client.PostAsJsonAsync("api/v1/LogIngestion", logEntry);
    
    if (!response.IsSuccessStatusCode)
    {
        string errorContent = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Error {(int)response.StatusCode} ({response.StatusCode}): {errorContent}");
        return;
    }
    
    response.EnsureSuccessStatusCode();
    Console.WriteLine("Test log sent successfully!");
}

async Task SendMultipleTestLogsAsync(HttpClient client)
{
    Console.Write("Enter number of logs to send: ");
    if (!int.TryParse(Console.ReadLine(), out int count) || count <= 0)
    {
        Console.WriteLine("Invalid number. Using default count of 5.");
        count = 5;
    }

    Console.WriteLine($"Sending {count} test logs...");

    var logs = new List<LogEntry>();
    for (int i = 0; i < count; i++)
    {
        logs.Add(CreateTestLogEntry(i));
    }

    var response = await client.PostAsJsonAsync("api/v1/LogIngestion/batch", logs);
    
    if (!response.IsSuccessStatusCode)
    {
        string errorContent = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Error {(int)response.StatusCode} ({response.StatusCode}): {errorContent}");
        return;
    }

    response.EnsureSuccessStatusCode();
    Console.WriteLine($"{count} test logs sent successfully!");
}

async Task GetAllLogsAsync(HttpClient client)
{
    Console.WriteLine("Getting all logs...");

    var logs = await client.GetFromJsonAsync<IEnumerable<LogEntry>>("api/v1/Logs");
    
    DisplayLogs(logs);
}

async Task GetLogsByApplicationAsync(HttpClient client)
{
    Console.Write("Enter application name: ");
    var appName = Console.ReadLine() ?? "TestApp";

    Console.WriteLine($"Getting logs for application '{appName}'...");

    var logs = await client.GetFromJsonAsync<IEnumerable<LogEntry>>($"api/v1/Logs/application/{appName}");
    
    DisplayLogs(logs);
}

async Task GetLogStatisticsAsync(HttpClient client)
{
    Console.WriteLine("Getting log statistics by application...");

    var appStats = await client.GetFromJsonAsync<Dictionary<string, int>>("api/v1/Logs/stats/applications");
    
    if (appStats != null && appStats.Count > 0)
    {
        Console.WriteLine("Logs by Application:");
        foreach (var appStat in appStats)
        {
            Console.WriteLine($"  {appStat.Key}: {appStat.Value} logs");
        }
    }
    else
    {
        Console.WriteLine("No statistics available.");
    }

    Console.WriteLine("\nGetting log statistics by level...");

    var levelStats = await client.GetFromJsonAsync<Dictionary<string, int>>("api/v1/Logs/stats/levels");
    
    if (levelStats != null && levelStats.Count > 0)
    {
        Console.WriteLine("Logs by Level:");
        foreach (var levelStat in levelStats)
        {
            Console.WriteLine($"  {levelStat.Key}: {levelStat.Value} logs");
        }
    }
    else
    {
        Console.WriteLine("No statistics available.");
    }
}

LogEntry CreateTestLogEntry(int index = 0)
{
    var levels = new[] { "Information", "Warning", "Error", "Debug", "Critical" };
    var apps = new[] { "TestApp", "WebService", "MobileApp", "BatchProcessor", "ApiGateway" };
    var environments = new[] { "Development", "Testing", "Staging", "Production" };
    
    var random = new Random();
    
    // Ensure required fields are always set and not empty
    return new LogEntry
    {
        Timestamp = DateTime.Now.AddMinutes(-random.Next(0, 60)),
        Level = levels[random.Next(levels.Length)],
        Message = $"Test log message {index} - {Guid.NewGuid()}",
        Application = apps[random.Next(apps.Length)],
        Environment = environments[random.Next(environments.Length)],
        MachineName = $"MACHINE-{random.Next(1, 10)}",
        Category = $"Category{random.Next(1, 5)}",
        Exception = random.Next(10) < 2 ? $"System.Exception: Test exception {index}" : "",
        StackTrace = random.Next(10) < 2 ? "   at TestMethod() in TestClass.cs:line 42" : "",
        AdditionalData = random.Next(10) < 3 ? JsonSerializer.Serialize(new { CustomProperty = $"Value {index}", TestValue = random.Next(100) }) : ""
    };
}

void DisplayLogs(IEnumerable<LogEntry>? logs)
{
    if (logs == null || !logs.Any())
    {
        Console.WriteLine("No logs found.");
        return;
    }

    Console.WriteLine($"Found {logs.Count()} logs:");
    
    foreach (var log in logs.Take(10))
    {
        Console.WriteLine(new string('-', 80));
        Console.WriteLine($"ID: {log.Id}");
        Console.WriteLine($"Timestamp: {log.Timestamp}");
        Console.WriteLine($"Level: {log.Level}");
        Console.WriteLine($"Application: {log.Application}");
        Console.WriteLine($"Environment: {log.Environment}");
        Console.WriteLine($"Message: {log.Message}");
        
        if (!string.IsNullOrEmpty(log.Exception))
        {
            Console.WriteLine($"Exception: {log.Exception}");
        }
    }
    
    if (logs.Count() > 10)
    {
        Console.WriteLine($"\n... and {logs.Count() - 10} more logs.");
    }
}
