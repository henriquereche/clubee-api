using Clubee.API.Contracts.Infrastructure.Storage;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System.Threading.Tasks;

namespace Clubee.API.Infrastructure.Storage
{
    public class ObjectStorageProvider : IObjectStorageProvider
    {
        private readonly StorageSettings StorageSettings;
        private readonly CloudBlobClient Client;

        public ObjectStorageProvider(StorageSettings storageSettings)
        {
            this.StorageSettings = storageSettings;
            this.Client = this.CreateClient();
        }

        /// <summary>
        /// Iniatilizes CloudBlobClient.
        /// </summary>
        /// <returns></returns>
        private CloudBlobClient CreateClient()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount
                .Parse(this.StorageSettings.Connection);

            return storageAccount.CreateCloudBlobClient();
        }

        /// <summary>
        /// Get blob object reference.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        private async Task<CloudBlockBlob> GetObject(string container, string objectName)
        {
            CloudBlobContainer blobContainer = this.Client.GetContainerReference(container);
            await blobContainer.CreateIfNotExistsAsync(
                BlobContainerPublicAccessType.Blob, null, null);

            return blobContainer.GetBlockBlobReference(objectName);
        }

        /// <summary>
        /// Inserts a new object.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="objectName"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public async Task<string> SetObject(string container, string objectName, byte[] buffer)
        {
            CloudBlockBlob blobObject = await this.GetObject(container, objectName);
            await blobObject.UploadFromByteArrayAsync(buffer, 0, buffer.Length);

            return blobObject.Uri.ToString();
        }

        /// <summary>
        /// Delete existing object.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        public async Task DeleteObject(string container, string objectName)
        {
            CloudBlockBlob blobObject = await this.GetObject(container, objectName);
            await blobObject.DeleteIfExistsAsync();
        }
    }
}
