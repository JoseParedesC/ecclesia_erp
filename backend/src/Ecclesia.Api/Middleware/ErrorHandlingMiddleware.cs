using System.Net;
using System.Text.RegularExpressions;
using Ecclesia.Domain.Common;
using Ecclesia.Domain.Common.Exceptions;
using FluentValidation;
using Microsoft.Extensions.Hosting;

namespace Ecclesia.Api.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger, IHostEnvironment env)
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

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var statusCode = HttpStatusCode.InternalServerError;
        var title = "Error inesperado.";
        var detail = ex.Message;
        IDictionary<string, string[]>? errors = null;

        // Extraer código de error del mensaje si existe [XXX-000]
        string? errorCode = null;
        var match = Regex.Match(ex.Message, @"\[([A-Z]+-\d+)\]");
        if (match.Success)
            errorCode = match.Groups[1].Value;

        switch (ex)
        {
            case NotFoundException notFoundEx:
                _logger.LogWarning(notFoundEx, "Recurso no encontrado.");
                statusCode = HttpStatusCode.NotFound;
                title = "No encontrado";
                break;

            case ValidationException fluentEx:
                _logger.LogWarning(fluentEx, "Validación fallida.");
                statusCode = HttpStatusCode.BadRequest;
                title = "Error de validación";
                detail = string.Join("; ", fluentEx.Errors.Select(e => e.ErrorMessage));
                errors = fluentEx.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );
                break;

            case DomainException domainEx:
                _logger.LogWarning(domainEx, "Violación de regla de dominio.");
                statusCode = HttpStatusCode.BadRequest;
                title = "Error de dominio";
                break;

            case BusinessException businessEx:
                _logger.LogWarning(businessEx, "Violación de regla de negocio.");
                statusCode = HttpStatusCode.UnprocessableEntity;
                title = "Error de negocio";
                break;

            case UnauthorizedAccessException unauthorizedEx:
                _logger.LogWarning(unauthorizedEx, "Acceso no autorizado.");
                statusCode = HttpStatusCode.Unauthorized;
                title = "No autorizado";
                detail = "No autorizado.";
                break;

            case ForbiddenException forbiddenEx:
                _logger.LogWarning(forbiddenEx, "Acceso prohibido.");
                statusCode = HttpStatusCode.Forbidden;
                title = "Prohibido";
                detail = "Acceso prohibido.";
                break;

            case BadHttpRequestException badReqEx:
                _logger.LogWarning(badReqEx, "Solicitud incorrecta: {Message}", badReqEx.Message);
                statusCode = HttpStatusCode.BadRequest;
                title = "Solicitud incorrecta";
                detail = "Formato de solicitud o tipo de dato inválido.";
                break;

            case InvalidOperationException invalidOpEx:
                _logger.LogWarning(invalidOpEx, "Operación inválida.");
                statusCode = HttpStatusCode.BadRequest;
                title = "Operación inválida";
                detail = ex.Message; // 👈 solo el mensaje, sin el tipo
                break;

            default:
                _logger.LogError(ex, "Excepción no controlada.");
                statusCode = HttpStatusCode.InternalServerError;
                title = "Error interno del servidor";
                detail = _env.IsDevelopment() ? ex.ToString() : "Ocurrió un error inesperado.";
                break;
        }

        await WriteResponseAsync(context, statusCode, title, detail, errors, errorCode);
    }

    private static async Task WriteResponseAsync(
        HttpContext context,
        HttpStatusCode statusCode,
        string title,
        string detail,
        IDictionary<string, string[]>? errors,
        string? errorCode)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)statusCode;

        var response = new ProblemDetailsResponse
        {
            Status = (int)statusCode,
            Title = title,
            Detail = detail,
            Type = $"https://httpstatuses.com/{(int)statusCode}",
            Instance = context.Request.Path,
            HasError = true,
            MsgError = detail,
            ErrorCode = errorCode,
            Errors = errors
        };

        await context.Response.WriteAsJsonAsync(response);
    }
}