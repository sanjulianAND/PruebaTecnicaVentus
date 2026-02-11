using Application.DTOs;
using Domain.Exceptions;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Text.Json;

namespace PruebaPracticaVentus.Middleware;

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
            _logger.LogInformation(
                "Solicitud {Method} {Path} iniciada",
                context.Request.Method,
                context.Request.Path);

            await _next(context);

            _logger.LogInformation(
                "Solicitud {Method} {Path} completada con status {StatusCode}",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error en {Method} {Path}. Tipo: {ExceptionType}. Mensaje: {Message}. StackTrace: {StackTrace}",
                context.Request.Method,
                context.Request.Path,
                ex.GetType().Name,
                ex.Message,
                ex.StackTrace);

            if (ex.InnerException != null)
            {
                _logger.LogError(
                    ex.InnerException,
                    "InnerException - Tipo: {ExceptionType}. Mensaje: {Message}",
                    ex.InnerException.GetType().Name,
                    ex.InnerException.Message);
            }

            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var (statusCode, mensaje) = exception switch
        {
            SqlException sqlEx => (
                HttpStatusCode.InternalServerError,
                $"Error de base de datos: {sqlEx.Message}. Número de error: {sqlEx.Number}"
            ),
            AutorNoEncontradoException => (HttpStatusCode.BadRequest, exception.Message),
            LibroNoEncontradoException => (HttpStatusCode.NotFound, exception.Message),
            MaximoLibrosException => (HttpStatusCode.BadRequest, exception.Message),
            ValidacionException => (HttpStatusCode.BadRequest, exception.Message),
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Acceso no autorizado"),
            InvalidOperationException => (HttpStatusCode.BadRequest, exception.Message),
            _ => (HttpStatusCode.InternalServerError, $"Error interno del servidor: {exception.Message}")
        };

        response.StatusCode = (int)statusCode;

        var errores = exception is ValidacionException validacionEx
            ? validacionEx.Errores
            : null;

        var resultado = RespuestaApi<object>.Error(mensaje, errores);

        return response.WriteAsync(JsonSerializer.Serialize(resultado, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }));
    }
}
