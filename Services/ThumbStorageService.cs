using Azure.Storage.Blobs.Models;
using ImageResize.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ImageResize.Services
{
    public class ThumbStorageService : StorageBaseService<ThumbStorageService>, IThumbStorageService
    {
        public ThumbStorageService(string connectionString, ILogger<ThumbStorageService> logger) : base(connectionString, "thumbs", logger)
        {
            CreateThumbsContainerIfNotExists();
        }

        async Task<ThumbUploadData> IThumbStorageService.Upload(ImageData thumbData)
        {
            try
            {
                var blob = _container.GetBlobClient(blobName: thumbData.Name);

                var options = new BlobUploadOptions
                {
                    HttpHeaders = new BlobHttpHeaders
                    {
                        ContentType = thumbData.ContentType,
                        ContentDisposition = thumbData.ContentDisposition
                    }
                };
                var response = await blob.UploadAsync(content: thumbData.Stream, options: options);

                return new ThumbUploadData
                {
                    ThumbUrl = blob.Uri.AbsoluteUri,
                    HasUploadedSuccessfuly = response.GetRawResponse().Status == 201
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(message: ex.Message);
                return null;
            }
        }

        private void CreateThumbsContainerIfNotExists()
        {
            try
            {
                _container.CreateIfNotExists(publicAccessType: PublicAccessType.BlobContainer);
            }
            catch (Exception ex)
            {
                _logger.LogError(message: ex.Message);
            }
        }
    }
}
