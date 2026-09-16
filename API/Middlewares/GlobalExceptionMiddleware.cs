using OrgSys.SharedKernel;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace API.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IHostEnvironment _environment;

        private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger,
            IHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {                
                await _next(context);
            }
            catch (AppValidationException ex)
            {
                await WriteResult(
                    context,
                    HttpStatusCode.BadRequest,
                    ex.Errors.ToList()
                );
            }
            catch (BadHttpRequestException ex)
            {
                await WriteResult(
                    context,
                    HttpStatusCode.BadRequest,
                    new List<Error> { new Error(ex.Message) }
                );
            }
            catch (ValidationException ex)
            {
                await WriteResult(
                    context,
                    HttpStatusCode.BadRequest,
                    ex.Errors.Select(e => new Error(e.ErrorMessage , e.PropertyName)).ToList()
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                await WriteProblem(context, ex);
            }
        }

        private static async Task WriteResult(
            HttpContext context,
            HttpStatusCode statusCode,
            List<Error> errors)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var result = new Result(statusCode, errors);

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(result, SerializerOptions)
            );
        }

        private async Task WriteProblem(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var correlationId = context.Items[CorrelationIdMiddleware.HeaderName] as string
                ?? context.TraceIdentifier;

            var problem = new ProblemDetails
            {
                Title = "An unexpected error occurred.",
                Status = StatusCodes.Status500InternalServerError,
                Detail = _environment.IsDevelopment() ? ex.Message : "An internal error occurred.",
                Instance = context.Request.Path,
            };
            problem.Extensions["traceId"] = context.TraceIdentifier;
            problem.Extensions["correlationId"] = correlationId;

            await context.Response.WriteAsync(JsonSerializer.Serialize(problem, SerializerOptions));
        }
    }
}
