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
                JsonSerializer.Serialize(result)
            );
        }
    }
}