using Application.Validators;
using Domain.Shared;
using FluentValidation;
using System.Linq;
using System.Net;
using System.Text.Json;

namespace API.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        // Every normal controller response goes through AddControllers().AddJsonOptions() in
        // Program.cs, which uses the default camelCase naming policy. JsonSerializer.Serialize()
        // called directly here (for exceptions that escape a handler's own try/catch — notably
        // FluentValidationFilter's pipeline-level AppValidationException) does NOT pick that up on
        // its own and previously fell back to PascalCase, silently breaking every camelCase-based
        // client-side error read (e.g. `result.errors[0].messageError`) for exactly the responses
        // meant to explain *why* a save failed.
        private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {                
                await _next(context);
            }
            catch (AppValidationException ex)
            {
                await WriteResponse(
                    context,
                    HttpStatusCode.BadRequest,
                    ex.Errors.ToList()
                );
            }
            catch (BadHttpRequestException ex)
            {
                await WriteResponse(
                    context,
                    HttpStatusCode.BadRequest,
                    new List<Error> { new Error(ex.Message) }
                );
            }
            catch (ValidationException ex)
            {
                await WriteResponse(
                    context,
                    HttpStatusCode.BadRequest,
                    ex.Errors.Select(e => new Error(e.ErrorMessage , e.PropertyName)).ToList()
                );
            }
            catch (Exception ex)
            {
                await WriteResponse(
                    context,
                    HttpStatusCode.InternalServerError,
                    new List<Error> { new Error(ex.Message) }
                );
            }
        }

        private static async Task WriteResponse(
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
    }
}