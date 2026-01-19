using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Application.Validators
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                var result = new Domain.Shared.Result(
                    HttpStatusCode.BadRequest,
                    errors
                );

                context.Result = new ObjectResult(result)
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };

                return;
            }

            await next();
        }
    }

}
