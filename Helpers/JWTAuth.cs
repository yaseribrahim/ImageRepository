using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ImageRepo.Helpers
{
    public static class JWTAuth
    {
        public static string GenerateJWT(string username, IConfiguration configuration)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtAuth:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(configuration["JwtAuth:Issuer"],
              configuration["JwtAuth:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static string GetUsername(ClaimsPrincipal claimsPrincipal){
            string username = null;
            if (claimsPrincipal.HasClaim(claim => claim.Type == ClaimTypes.NameIdentifier))
            {
                username = claimsPrincipal.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
            }
            return username;
        }
    }
}