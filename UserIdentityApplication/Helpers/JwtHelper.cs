using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserIdentityApplication.Models;

namespace UserIdentityApplication.Helpers
{
    /// <summary>
    /// Provides helper methods for generating JSON Web Tokens (JWTs).
    /// </summary>
    public static class JwtHelper
    {
        /// <summary>
        /// Generates a JWT token for the given user with assigned roles.
        /// </summary>
        /// <param name="user">The user entity containing username and email details.</param>
        /// <param name="roles">A list of roles assigned to the user.</param>
        /// <param name="configuration">Application configuration used to retrieve JWT settings (Key, Issuer, Audience).</param>
        /// <returns>A signed JWT token as a string.</returns>
        public static string GenerateToken(Users user, List<string> roles, IConfiguration configuration)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            // Add role claims
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
