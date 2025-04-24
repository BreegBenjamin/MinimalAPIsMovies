using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using MinimalAPIsMovies.Services;

namespace MinimalAPIsMovies.Utilities
{
    public class SecretService : ISecretService
    {
        private readonly SecretClient _secretClient;

        public SecretService(IConfiguration config)
        {
            var keyVaultUrl = config["KeyVault:Url"];
            _secretClient = new SecretClient(new Uri(keyVaultUrl!), new DefaultAzureCredential());
        }

        public async Task<string> GetSecretAsync(string secretName)
        {
            var secret = await _secretClient.GetSecretAsync(secretName);
            return secret.Value.Value;
        }
    }
}
