using Microsoft.IdentityModel.Tokens;
using MinimalAPIsMovies.Services;

namespace MinimalAPIsMovies.Utilities
{
    public class KeysHandler
    {
        public const string OurIssuer = "our-app";
        private const string KeysSection = "Authentication:Schemes:Bearer:SigningKeys";
        private const string KeysSection_Issuer = "signing-key-issuer";
        private const string KeysSection_Value = "signing-key-value";

        public static async Task<SecurityKey> GetKeyFromSecret(ISecretService secretService) 
        {
            string secretKey = await secretService.GetSecretAsync(KeysSection_Value);

            return new SymmetricSecurityKey(Convert.FromBase64String(secretKey?? string.Empty));
        }

        public static IEnumerable<SecurityKey> GetKey(IConfiguration configuration,
            string issuer)
        {
            var signingKey = configuration.GetSection(KeysSection)
                .GetChildren()
                .SingleOrDefault(key => key[KeysSection_Issuer] == issuer);

            if (signingKey is not null && signingKey[KeysSection_Value] is string secretKey)
            {
                yield return new SymmetricSecurityKey(Convert.FromBase64String(secretKey));
            }
        }

        public static IEnumerable<SecurityKey> GetAllKeys(IConfiguration configuration)
        {
            var signingKeys = configuration.GetSection(KeysSection)
                .GetChildren();

            foreach (var signingKey in signingKeys)
            {
                if (signingKey[KeysSection_Value] is string secretKey)
                {
                    yield return new SymmetricSecurityKey(Convert.FromBase64String(secretKey));
                }
            }
        }
    }
}
