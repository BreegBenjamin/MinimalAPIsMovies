
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Path = System.IO.Path;

namespace MinimalAPIsMovies.Services
{
    public class AzureFileStorage(ISecretService secretService) : IFileStorage
    {
        const string BLOB_KEY = "blob-connection-string";
        public async Task Delete(string? route, string container)
        {
            string connectionString = await secretService.GetSecretAsync(BLOB_KEY);
            if (string.IsNullOrEmpty(route) || string.IsNullOrEmpty(connectionString))
            {
                return;
            }

            var client = new BlobContainerClient(connectionString, container);
            await client.CreateIfNotExistsAsync();
            var fileName = Path.GetFileName(route);
            var blob = client.GetBlobClient(fileName);
            await blob.DeleteIfExistsAsync();
        }

        public async Task<string> Store(string container, IFormFile file)
        {
            string connectionString = await secretService.GetSecretAsync(BLOB_KEY);
            if (string.IsNullOrEmpty(connectionString))
            {
                return string.Empty;
            }

            var client = new BlobContainerClient(connectionString, container);
            await client.CreateIfNotExistsAsync();
            client.SetAccessPolicy(PublicAccessType.Blob);

            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";
            var blob = client.GetBlobClient(fileName);
            BlobHttpHeaders blobHttpHeaders = new();
            blobHttpHeaders.ContentType = file.ContentType;
            await blob.UploadAsync(file.OpenReadStream(), blobHttpHeaders);
            return blob.Uri.ToString();
        }
    }
}
