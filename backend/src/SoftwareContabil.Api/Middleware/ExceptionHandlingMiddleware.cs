using System.Net;
using System.Text.Json;
using SoftwareContabil.Domain.Exceptions;

namespace SoftwareContabil.Api.Middleware;

/// <summary>
/// Único ponto de tradução entre exceções de domínio e respostas HTTP.
/// Controllers e serviços apenas lançam <see cref="NotFoundException"/> ou
/// <see cref="BusinessRuleException"/> — nenhum deles precisa saber qual
/// código HTTP corresponde a cada caso (Single Responsibility).
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            await WriteProblemAsync(context, HttpStatusCode.NotFound, ex.Message);
        }
        catch (BusinessRuleException ex)
        {
            await WriteProblemAsync(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro não tratado ao processar {Path}", context.Request.Path);
            await WriteProblemAsync(context, HttpStatusCode.InternalServerError, "Ocorreu um erro inesperado ao processar a requisição.");
        }
    }

    private static async Task WriteProblemAsync(HttpContext context, HttpStatusCode status, string message)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)status;
        var payload = JsonSerializer.Serialize(new { status = (int)status, title = message });
        await context.Response.WriteAsync(payload);
    }
}

public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseAppExceptionHandling(this IApplicationBuilder app)
        => app.UseMiddleware<ExceptionHandlingMiddleware>();
}
