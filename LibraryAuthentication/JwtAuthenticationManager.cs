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
        public static Auth GenerateJwtToken(LoginRequest login)
        {
            try
            {
                var tokenExpiryTimeStamp = DateTime.Now.AddMinutes(JWT_TOKEN_VALIDITY_MINS);
                var tokenKey = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);
                var claimsIdentity = new ClaimsIdentity(new List<Claim>
                {
                    new Claim("UserName", login.UserNname),
                });
                    
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


                return new Auth
                {
                    token = token,
                    expiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.Now).TotalSeconds
                };
            }
            catch (Exception)
            {
                return new Auth
                {
                    expiresIn = 0,
                    token = string.Empty

                };
            }
        }
    }
}
