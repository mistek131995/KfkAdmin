using KfkAdmin.Infrastructure.Database;

namespace KfkAdmin.Services.Logger;

public class DatabaseLoggerProvider(SqLiteContext context) : ILoggerProvider
{
    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new DatabaseLogger(context);
    }
}