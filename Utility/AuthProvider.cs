using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;

namespace Utility
{
    public class AuthProvider
    {
        public static bool HasPermissions(string permissions)
        {
            var role= ClaimsPrincipal.Current.Identities.First().Claims.ToList();
            //var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

            //if (identity == null)
            //{
            //    return false;
            //}

            //var identityClaims = identity.Claims.Where(c => c.Type == ClaimTypes.Webpage)
            //                   .Select(c => c.Value).SingleOrDefault();

            //if (identityClaims == null)
            //{
            //    return false;
            //}

            //string[] permissionsArr = permissions.Split(',');

            //var identityClaimsArr = identityClaims.Split(',');

            //foreach (var per in permissionsArr)
            //{
            //    if (identityClaimsArr.Contains(per))
            //    {
            //        return true;
            //    }
            //}

            return false;



        }
    }
}
