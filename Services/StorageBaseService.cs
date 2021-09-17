using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using System;

namespace ImageResize.Services
{
    public abstract class StorageBaseService<T>
    {
        protected readonly ILogger<T> _logger;
        protected readonly BlobContainerClient _container;

        public StorageBaseService(string connectionString, string containerName, ILogger<T> logger)
        {
            _logger = logger;
            _container = InstantiateContainer(connectionString, containerName);
        }

        private BlobContainerClient InstantiateContainer(string connectionString, string containerName)
        {
            try
            {
                if (string.IsNullOrEmpty(connectionString)) throw new ArgumentException("Can't be empty", nameof(connectionString));
                if (string.IsNullOrEmpty(containerName)) throw new ArgumentException("Can't be empty", nameof(containerName));

                var container = new BlobContainerClient(connectionString, blobContainerName: containerName);
                return container;
            }
            catch (Exception ex)
            {
                _logger.LogError(message: ex.Message);
                return null;
            }
        }
    }
}
