using LibraryAuthentication.Encryption;
using LibraryEntities;
using LibraryEntities.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibraryAuthentication
{
    public class JwtTokenHandler
    {
        public const string JWT_SECURITY_KEY = "yPkCqn4kSWLtaJwXvN2jGzpQRyTZ3gdXkt7FeBJP";
        private const int JWT_TOKEN_VALIDITY_MINS = 1440;

        /// <summary>
        /// Generación de Token 
        /// </summary>
        /// <param name="authenticationRequest">Autenticación (Company, Username, password)</param>
        /// <returns>Token Generado</returns>
        public Response<Auth> GenerateJwtToken(LoginRequest login, long userId, string roleName)
        {
            try
            {
                var tokenExpiryTimeStamp = DateTime.Now.AddMinutes(JWT_TOKEN_VALIDITY_MINS);
                var tokenKey = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);
                // Build claims: include user id as Name, username and role(s)
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userId.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim("UserName", login.UserNname ?? string.Empty)
                };

                if (!string.IsNullOrWhiteSpace(roleName))
                {
                    var normalizedRole = roleName;
                    // Normalize common admin role names to "Admin" for attribute checks
                    if (roleName.IndexOf("admin", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        roleName.IndexOf("administrador", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        normalizedRole = "Admin";
                    }

                    claims.Add(new Claim(ClaimTypes.Role, normalizedRole));
                    claims.Add(new Claim("role", normalizedRole));
                }

                var claimsIdentity = new ClaimsIdentity(claims);
                    
                var signingCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature);

                var securityTokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claimsIdentity,
                    NotBefore = tokenExpiryTimeStamp.AddMinutes(-JWT_TOKEN_VALIDITY_MINS),
                    Expires = tokenExpiryTimeStamp,
                    SigningCredentials = signingCredentials
                };

                var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
                var token = jwtSecurityTokenHandler.WriteToken(securityToken);


                return new Response<Auth>
                {
                    status = System.Net.HttpStatusCode.OK,
                    response = new Auth
                    {
                        token = token,
                        expiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.Now).TotalSeconds
                    }
                };
            }
            catch (Exception)
            {
                return new Response<Auth>
                {
                    status = System.Net.HttpStatusCode.InternalServerError,
                    response = null
                };
            }
        }
    }
}
