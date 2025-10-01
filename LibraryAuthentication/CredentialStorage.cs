using Microsoft.Extensions.Configuration;

namespace LibraryAuthentication
{
    public class CredentialStorage
    {
        public static Dictionary<string, string> Credentials { get; private set; }

        public static void Initialize(IConfiguration configuration)
        {
            var credentialsSection = configuration.GetSection("AuthCredentials");
            Credentials = new Dictionary<string, string>();
            foreach (var item in credentialsSection.GetChildren())
            {
                Credentials[item.Key] = item.Value;
            }
        }
    }
}
