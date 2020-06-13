using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using System.Threading.Tasks;

namespace StaticFileFunction.Storage
{
    public class StorageService : IStorageService
    {
        private readonly CloudBlobContainer _container;
        private readonly string _storageContainerPrefix;

        public StorageService(CloudStorageAccount account, IConfiguration configuration)
        {
            var containerName = configuration["StorageContainerName"];
            var blobClient = account.CreateCloudBlobClient();
            _container = blobClient.GetContainerReference(containerName);

            _storageContainerPrefix = _container.Uri.ToString();
            if (!_storageContainerPrefix.EndsWith("/"))
                _storageContainerPrefix += "/";
        }

        public async Task<bool> FileExistsAsync(string path)
        {
            // can't use that or it won't work
            if (path.StartsWith(_storageContainerPrefix))
                path = path.Substring(_storageContainerPrefix.Length);

            await _container.CreateIfNotExistsAsync();
            return await _container.GetBlockBlobReference(path).ExistsAsync();
        }

        public async Task<Stream> ReadAsync(string path)
        {
            // can't use that or it won't work
            if (path.StartsWith(_storageContainerPrefix))
                path = path.Substring(_storageContainerPrefix.Length);

            await _container.CreateIfNotExistsAsync();
            var blob = _container.GetBlockBlobReference(path);
            return await blob.OpenReadAsync();
        }
    }
}
