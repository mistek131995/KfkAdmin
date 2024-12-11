namespace KfkAdmin.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            // Логирование ошибки или возврат пользовательского ответа
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Произошла ошибка: " + ex.Message);
        }
    }
}