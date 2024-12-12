using Microsoft.Extensions.Logging;

namespace KfkAdmin.Services.Logger;

public class DatabaseLoggerProvider : ILoggerProvider
{
    private readonly IServiceProvider _serviceProvider;

    public DatabaseLoggerProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new DatabaseLogger(_serviceProvider, categoryName);
    }

    public void Dispose()
    {
        // Освобождение ресурсов, если требуется
    }
}