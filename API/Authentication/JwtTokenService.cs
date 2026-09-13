using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Authentication
{
    public class JwtTokenService(IOptions<JwtOptions> options) : IJwtTokenService
    {
        private readonly JwtOptions _options = options.Value;

        public string GenerateToken(UserDto user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.UserName),
                new("displayName", user.Name),
                new(ClaimTypes.Role, user.RoleId.ToString()),
                new("roleId", user.RoleId.ToString()),
                new("roleName", user.RoleName ?? string.Empty),
            };

            if (user.BranchId.HasValue)
                claims.Add(new Claim("branchId", user.BranchId.Value.ToString()));

            if (user.Permissions is not null)
            {
                foreach (var permission in user.Permissions)
                {
                    if (!string.IsNullOrWhiteSpace(permission.Key))
                        claims.Add(new Claim("permission", permission.Key));
                }
            }

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
