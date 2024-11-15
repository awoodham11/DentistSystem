using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace BlazorApp1.Identity.Services
{
    public class JwtParser
    {
        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);

            var claims = new List<Claim>();

            foreach (var claim in token.Claims)
            {
                // Handle role claims specifically
                if (claim.Type == "role" || claim.Type == ClaimTypes.Role)
                {
                    claims.Add(new Claim(ClaimTypes.Role, claim.Value));

                    // Check if the role claim is a JSON array
                    if (claim.Value.StartsWith("[") && claim.Value.EndsWith("]"))
                    {
                        // Parse the JSON array and add each role as a separate claim
                        var roles = JArray.Parse(claim.Value).Values<string>();
                        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
                    }
                    else
                    {
                        // Add single role directly
                        claims.Add(new Claim(ClaimTypes.Role, claim.Value));
                    }
                }
                else
                {
                    // For other claims, add them as they are
                    claims.Add(claim);
                }
            }

            return claims;
        }
    }
}
