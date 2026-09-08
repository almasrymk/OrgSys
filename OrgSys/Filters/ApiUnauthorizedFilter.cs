using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace OrgSys.Filters
{
    // Every screen calls the API with the JWT stored in the auth cookie's ApiToken claim.
    // That claim is missing on cookies issued before this claim existed, and the JWT itself
    // expires independently of the cookie - either case makes the API reject the call with
    // 401, which today surfaces as an unhandled HttpRequestException. Sign the stale session
    // out and send the user back to the login page instead of crashing the request.
    public class ApiUnauthorizedFilter : IAsyncExceptionFilter
    {
        public async Task OnExceptionAsync(ExceptionContext context)
        {
            if (context.Exception is not HttpRequestException httpEx || httpEx.StatusCode != HttpStatusCode.Unauthorized)
                return;

            await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var returnUrl = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;
            context.Result = new RedirectToActionResult("LogIn", "Home", new { ReturnUrl = returnUrl });
            context.ExceptionHandled = true;
        }
    }
}
