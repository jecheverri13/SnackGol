using System;
using Microsoft.AspNetCore.Http;

namespace MSSnackGolFrontend.Infrastructure
{
    public static class SessionTokenHelper
    {
        private const string SessionKey = "SessionToken";

        public static string GetOrCreate(HttpContext httpContext)
        {
            if (!httpContext.Session.TryGetValue(SessionKey, out _))
            {
                var token = Guid.NewGuid().ToString("N");
                httpContext.Session.SetString(SessionKey, token);
            }

            return httpContext.Session.GetString(SessionKey)!;
        }
    }
}
