namespace MinimalAPIsMovies.Services
{
    public interface ISecretService
    {
        Task<string> GetSecretAsync(string secretName);
    }
}
