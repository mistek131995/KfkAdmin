using KfkAdmin.Infrastructure.Database;
using KfkAdmin.Infrastructure.Database.Tables;
using Microsoft.EntityFrameworkCore;

namespace KfkAdmin.Services.Logger;

public class DbLogger(IServiceProvider serviceProvider) : ILogger
{
    public IDisposable BeginScope<TState>(TState state) => null;

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel >= LogLevel.Information;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }
        
        
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<SqLiteContext>();
        dbContext.Database.Migrate();

        var logEntry = new Log
        {
            Date = DateTime.UtcNow,
            LogLevel = logLevel.ToString(),
            Message = exception?.Message ?? string.Empty,
            StackTrace = exception?.StackTrace ?? string.Empty,
            Source = exception?.Source ?? string.Empty,
        };

        dbContext.Logs.Add(logEntry);
        dbContext.SaveChanges();
    }
}