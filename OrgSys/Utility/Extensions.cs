using Application.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace OrgSys
{
    public static class Extensions
    {
        public static bool IsAllowed(this ClaimsPrincipal ob, string permissions)
        {
            var identity = ob;

            if (identity == null)
            {
                return false;
            }

            var identityClaimsOwner = identity.Claims.FirstOrDefault(c => c.Type == "RoleId")?.Value;
            if (long.Parse("0" + identityClaimsOwner) == 1)
                return true;

            var identityClaims = identity.Claims.Where(c => c.Type == ClaimTypes.Webpage)
                               .Select(c => c.Value).SingleOrDefault();

            if (identityClaims == null)
            {
                return false;
            }

            string[] permissionsArr = permissions.Split(',');

            var identityClaimsArr = identityClaims.Split(',');

            foreach (var per in permissionsArr)
            {
                if (identityClaimsArr.Any( e=> e == per))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsCurrentRole(this ClaimsPrincipal ob, long roleId)
        {
            var identity = ob;

            if (identity == null)
            {
                return false;
            }

            var identityClaims = identity.Claims.FirstOrDefault(c => c.Type == "RoleId")?.Value;
            return long.Parse("0" + identityClaims) == roleId;
        }

        public static bool IsCurrentUserAndRole(this ClaimsPrincipal ob, long userId, long roleId)
        {
            var identity = ob;

            if (identity == null)
            {
                return false;
            }

            var identityClaims = identity.Claims.FirstOrDefault(c => c.Type == "RoleId")?.Value;
            var identityClaimsUser = identity.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            return long.Parse("0" + identityClaims) == roleId && long.Parse("0" + identityClaimsUser) == userId;
        }

        public static long GetRoleId(this ClaimsPrincipal ob)
        {
            var identity = ob;

            if (identity == null)
            {
                return 0;
            }

            var identityClaims = identity.Claims.FirstOrDefault(c => c.Type == "RoleId")?.Value;
            return long.Parse("0" + identityClaims);
        }

        public static bool IsOwner(this ClaimsPrincipal ob)
        {
            var identity = ob;

            if (identity == null)
            {
                return false;
            }

            var identityClaims = identity.Claims.FirstOrDefault(c => c.Type == "RoleId")?.Value;
            return long.Parse("0" + identityClaims) == 1;
        }
        
        public static long GetUserId(this ClaimsPrincipal ob)
        {
            var identity = ob;

            if (identity == null)
            {
                return 0;
            }

            var identityClaims = identity.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            return long.Parse("0" + identityClaims);
        }

        public static string GetUserName(this ClaimsPrincipal ob)
        {
            var identity = ob;

            if (identity == null)
            {
                return "";
            }

            var identityClaims = identity.Claims.FirstOrDefault(c => c.Type == "UserName")?.Value;
            return "" + identityClaims;
        }

        public static string GetName(this ClaimsPrincipal ob)
        {
            var identity = ob;

            if (identity == null)
            {
                return "";
            }

            var identityClaims = identity.Claims.FirstOrDefault(c => c.Type == "Name")?.Value;
            return "" + identityClaims;
        }

        public static string GetSchema(this ClaimsPrincipal ob)
        {
            var identity = ob;

            if (identity == null)
            {
                return "";
            }

            var identityClaims = identity.Claims.FirstOrDefault(c => c.Type == "Schema")?.Value;
            return "" + identityClaims;
        }

        public static string GetApiToken(this ClaimsPrincipal ob)
        {
            var identity = ob;

            if (identity == null)
            {
                return "";
            }

            var identityClaims = identity.Claims.FirstOrDefault(c => c.Type == "ApiToken")?.Value;
            return "" + identityClaims;
        }

        public static string GetImage(this ClaimsPrincipal ob)
        {
            var identity = ob;

            if (identity == null)
            {
                return "";
            }

            var identityClaims = identity.Claims.FirstOrDefault(c => c.Type == "ImgPath")?.Value;
            return "" + identityClaims;
        }
        public static bool SignUp(this LoginUserDto us)
        {
            try
            {
                
                return true;
            }
            catch
            {
                return false;
                throw;
            }
        }

        public static bool ConfirmSignUp(string Code)
        {
            try
            {

                return true;
            }
            catch
            {
                return false;
                throw;
            }
        }

        public static bool SignIn(this UserDto us, HttpContext httpContext , string Schema , bool KeepMeLoggedin = true, string ApiToken = null)
        {
            try
            {
                // Re-sign-in flows (e.g. after a profile/permissions update) call this without a
                // fresh ApiToken, so carry the token issued at login forward instead of dropping it -
                // losing it here would make every subsequent API call fail with 401.
                var token = string.IsNullOrEmpty(ApiToken) ? httpContext.User.GetApiToken() : ApiToken;

                var claims = new List<Claim>()
                    {
                      new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "" + us.Name ),
                      new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "" + us.UserName),
                      new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "Organizer"),
                      new Claim("Name",  "" + us.Name) ,
                      new Claim("UserName",  "" + us.UserName) ,
                      new Claim("RoleId", us.RoleId.ToString()),
                      new Claim("Schema", Schema),
                      new Claim(ClaimTypes.Role,  "" + us.RoleName) ,
                      new Claim(ClaimTypes.Webpage, us.RoleId == 1 ? "" :  string.Join(",",  us.Permissions.Select(r=>r.Key).ToList())),
                      new Claim("Id", us.Id.ToString()),
                      new Claim("ImgPath", "" + us.ImgPath),
                      new Claim("ApiToken", "" + token),
                   };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties() { IsPersistent = KeepMeLoggedin };                
                httpContext.SignOutAsync();
                httpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                return true;
            }
            catch
            {
                return false;
                throw;
            }
        }

        public static bool SignOut(this LoginUserDto us, HttpContext httpContext)
        {
            try
            {               
                httpContext.SignOutAsync();               
                return true;
            }
            catch
            {
                return false;
                throw;
            }
        }

        //public static void AddUpdateClaim(this IPrincipal currentPrincipal, HttpContext httpContext, string key, string value)
        //{
        //    var identity = currentPrincipal.Identity as ClaimsIdentity;
        //    if (identity == null)
        //        return;

        //    // check for existing claim and remove it
        //    var existingClaim = identity.FindFirst(key);
        //    if (existingClaim != null)
        //        identity.RemoveClaim(existingClaim);

        //    // add new claim
        //    identity.AddClaim(new Claim(key, value));
        //    var authenticationManager = httpContext.Request.GetOwinContext().Authentication;
        //    authenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(new ClaimsPrincipal(identity), new AuthenticationProperties() { IsPersistent = true });
        //}
    }
}