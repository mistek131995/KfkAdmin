namespace KfkAdmin.Services.Logger;

public class DbLoggerProvider(IServiceProvider serviceProvider) : ILoggerProvider
{
    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new DbLogger(serviceProvider);
    }
}