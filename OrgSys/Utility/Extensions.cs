using Entity.ModelView;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
                if (identityClaimsArr.Contains(per))
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
        public static bool SignUp(this UserModelView us)
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

        public static bool SignIn(this UserModelView us, HttpContext httpContext)
        {
            try
            {
                var claims = new List<Claim>()
                    {
                      new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "" + us.Name ),
                      new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "" + us.UserName),
                      new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "Organizer"),
                      new Claim("Name",  "" + us.Name) ,
                      new Claim("RoleId", us.RoleId.ToString()),
                      new Claim(ClaimTypes.Role,  "" + us.RoleName) ,
                      new Claim(ClaimTypes.Webpage,  string.Join(",",  us.Permissions.Select(r=>r.Key).ToList())),
                      new Claim("Id", us.Id.ToString()),
                      new Claim("ImgPath", "" + us.ImgPath),
                   };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties();
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
    }
}
