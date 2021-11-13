using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UDemyAuth.Models;

namespace LoginWithIdentity.ClaimProvider
{
    public class ClaimProvider : IClaimsTransformation
    {

        public UserManager<AppUser> _userManager { get; }

        public ClaimProvider(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }



        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {

            if (principal!=null && principal.Identity.IsAuthenticated)
            {
                ClaimsIdentity identity = principal.Identity as ClaimsIdentity;

                var user = await _userManager.FindByNameAsync(identity.Name);

                if (user!=null)
                {
                    if (user.City!=null)
                    {
                        if (!principal.HasClaim(c=>c.Type=="city"))
                        {
                            Claim claim = new Claim("city", user.City, ClaimValueTypes.String, "Local");

                            identity.AddClaim(claim);
                        }
                    }
                }

            }

            return principal;

        }
    }
}
