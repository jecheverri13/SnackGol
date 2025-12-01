using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

namespace LibraryAuthentication
{
    public static class CustomJwtAuthExtension
    {
        public static void AddCustomJwtAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JwtTokenHandler.JWT_SECURITY_KEY)),
                    // Map role/name claim types to standard claims so [Authorize(Roles=..)] works
                    NameClaimType = ClaimTypes.Name,
                    RoleClaimType = ClaimTypes.Role
                };

                // Add event handlers to customize responses
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        // Skip the default behavior
                        context.HandleResponse();

                        // Create custom response
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";

                        var response = new
                        {
                            Success = false,
                            Message = "Unauthorized",
                            Data = new object()
                        };

                        var responseJson = System.Text.Json.JsonSerializer.Serialize(response);

                        // Write the response
                        return context.Response.WriteAsync(responseJson);
                    }
                };
            });
        }
    }
}
