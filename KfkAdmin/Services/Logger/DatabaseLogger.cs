using KfkAdmin.Infrastructure.Database;
using KfkAdmin.Infrastructure.Database.Tables;

namespace KfkAdmin.Services.Logger;

public class DatabaseLogger(SqLiteContext context) : ILogger
{
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        throw new NotImplementedException();
    }

    public bool IsEnabled(LogLevel logLevel)
    { 
        return logLevel > LogLevel.Warning;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel)) return;

        context.Logs.Add(new Log()
        {
            LogLevel = logLevel,
            Date = DateTime.UtcNow,
            Message = exception?.Message ?? string.Empty,
            StackTrace = exception?.StackTrace ?? string.Empty,
            Source = exception?.Source ?? string.Empty,
        });
        
        context.SaveChanges();
    }
}