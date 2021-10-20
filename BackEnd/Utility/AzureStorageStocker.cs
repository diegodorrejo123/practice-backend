using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Utility
{
    public class AzureStorageStocker : IFileStocker
    {
        private string connectionString;
        public AzureStorageStocker(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("AzureStorage");
        }

        public async Task<string> storeFile(string container, IFormFile file)
        {
            var client = new BlobContainerClient(connectionString, container);
            await client.CreateIfNotExistsAsync();
            client.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";
            var blob = client.GetBlobClient(fileName);
            await blob.UploadAsync(file.OpenReadStream());
            return blob.Uri.ToString();
        }

        public async Task deleteFile(string route, string container)
        {
            if (string.IsNullOrEmpty(route))
            {
                return;
            }
            var client = new BlobContainerClient(connectionString, container);
            await client.CreateIfNotExistsAsync();

            var file = Path.GetFileName(route);
            var blob = client.GetBlobClient(file);
            await blob.DeleteIfExistsAsync();
        }

        public async Task<string> updateFile(string container, IFormFile file, string route)
        {
            await deleteFile(route, container);
            return await storeFile(container, file);
        }
    }
}
