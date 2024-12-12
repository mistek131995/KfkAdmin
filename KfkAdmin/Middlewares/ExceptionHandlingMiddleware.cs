namespace KfkAdmin.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogWarning(message: ex.Message, exception: ex);
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Произошла ошибка: " + ex.Message);
        }
    }
}