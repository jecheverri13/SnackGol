using System;
using Microsoft.AspNetCore.Http;

namespace MSSnackGolFrontend.Infrastructure
{
    public static class SessionTokenHelper
    {
        private const string SessionKey = "SessionToken";
        private const string CookieKey = "SnackGol_SessionToken";

        /// <summary>
        /// Obtiene o crea un token de sesión persistente.
        /// El token se almacena tanto en la sesión como en una cookie persistente
        /// para sobrevivir reinicios del navegador.
        /// </summary>
        public static string GetOrCreate(HttpContext httpContext)
        {
            // 1. Primero intentar obtener de la sesión (más rápido)
            var token = httpContext.Session.GetString(SessionKey);
            
            // 2. Si no está en sesión, intentar recuperar de cookie persistente
            if (string.IsNullOrEmpty(token))
            {
                if (httpContext.Request.Cookies.TryGetValue(CookieKey, out var cookieToken) 
                    && !string.IsNullOrEmpty(cookieToken))
                {
                    token = cookieToken;
                    // Restaurar en la sesión
                    httpContext.Session.SetString(SessionKey, token);
                }
            }
            
            // 3. Si no existe en ningún lado, crear nuevo token
            if (string.IsNullOrEmpty(token))
            {
                token = Guid.NewGuid().ToString("N");
                httpContext.Session.SetString(SessionKey, token);
            }
            
            // 4. Asegurar que la cookie persistente esté actualizada
            EnsurePersistentCookie(httpContext, token);
            
            return token;
        }

        /// <summary>
        /// Crea o actualiza la cookie persistente con el token de sesión.
        /// La cookie dura 30 días para mantener el historial de pedidos del usuario.
        /// </summary>
        private static void EnsurePersistentCookie(HttpContext httpContext, string token)
        {
            // Verificar si la cookie ya existe con el mismo valor
            if (httpContext.Request.Cookies.TryGetValue(CookieKey, out var existingToken) 
                && existingToken == token)
            {
                return; // Cookie ya está actualizada
            }

            // Crear o actualizar la cookie
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = httpContext.Request.IsHttps,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddDays(30), // 30 días de persistencia
                Path = "/"
            };

            httpContext.Response.Cookies.Append(CookieKey, token, cookieOptions);
        }

        /// <summary>
        /// Obtiene el token actual sin crear uno nuevo. Retorna null si no existe.
        /// </summary>
        public static string? Get(HttpContext httpContext)
        {
            var token = httpContext.Session.GetString(SessionKey);
            
            if (string.IsNullOrEmpty(token) 
                && httpContext.Request.Cookies.TryGetValue(CookieKey, out var cookieToken))
            {
                return cookieToken;
            }
            
            return token;
        }
    }
}
