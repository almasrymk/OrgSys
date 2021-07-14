using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Utility
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
    }
}
