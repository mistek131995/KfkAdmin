using KfkAdmin.Infrastructure.Database;
using KfkAdmin.Infrastructure.Database.Tables;
using Microsoft.Extensions.Logging;

namespace KfkAdmin.Services.Logger;

public class DatabaseLogger : ILogger
{
    private readonly IServiceProvider _serviceProvider;
    private readonly string _categoryName;

    public DatabaseLogger(IServiceProvider serviceProvider, string categoryName)
    {
        _serviceProvider = serviceProvider;
        _categoryName = categoryName;
    }

    public IDisposable BeginScope<TState>(TState state) => null;

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel >= LogLevel.Warning;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        if (!IsEnabled(logLevel)) return;

        try
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SqLiteContext>();

            var logEntry = new Log
            {
                Date = DateTime.UtcNow,
                LogLevel = logLevel,
                Message = exception?.Message ?? String.Empty,
                Source = _categoryName,
                StackTrace = exception?.StackTrace ?? String.Empty,
            };

            dbContext.Logs.Add(logEntry);
            dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка записи лога: {ex.Message}");
        }
    }
}