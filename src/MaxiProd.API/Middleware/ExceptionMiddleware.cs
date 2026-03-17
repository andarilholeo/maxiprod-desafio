using System.Net;
using System.Text.Json;
using Npgsql;

namespace MaxiProd.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var (statusCode, message, details) = GetExceptionDetails(exception);

        response.StatusCode = (int)statusCode;

        _logger.LogError(exception, "Erro capturado pelo middleware: {Message}", exception.Message);

        var result = JsonSerializer.Serialize(new
        {
            error = message,
            details = _env.IsDevelopment() ? details : null,
            timestamp = DateTime.UtcNow
        }, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        await response.WriteAsync(result);
    }

    private (HttpStatusCode statusCode, string message, string? details) GetExceptionDetails(Exception exception)
    {
        return exception switch
        {
            NpgsqlException npgsqlEx => (
                HttpStatusCode.ServiceUnavailable,
                "Erro de conexão com o banco de dados. O PostgreSQL pode não estar rodando ou as credenciais estão incorretas. 🐘💔",
                $"NpgsqlException: {npgsqlEx.Message}"
            ),

            InvalidOperationException invOpEx when invOpEx.Message.Contains("database") => (
                HttpStatusCode.ServiceUnavailable,
                "Erro de operação com o banco de dados. Verifique se o PostgreSQL está acessível.",
                $"InvalidOperationException: {invOpEx.Message}"
            ),

            TimeoutException => (
                HttpStatusCode.RequestTimeout,
                "O servidor demorou demais para responder. Tente novamente em instantes. ⏱️",
                "Timeout na requisição"
            ),

            ArgumentException argEx => (
                HttpStatusCode.BadRequest,
                "Argumento inválido na requisição.",
                $"ArgumentException: {argEx.Message}"
            ),

            _ => (
                HttpStatusCode.InternalServerError,
                "Ocorreu um erro inesperado no servidor. Nossa equipe foi notificada (mentira, somos só devs aqui 😅).",
                $"{exception.GetType().Name}: {exception.Message}"
            )
        };
    }
}

public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionMiddleware>();
    }
}

