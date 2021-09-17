using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using ImageResize.Models;

namespace ImageResize.Services
{
    public class ImageStorageService : StorageBaseService<ImageStorageService>, IImageStorageService
    {
        public ImageStorageService(string connectionString, ILogger<ImageStorageService> logger) : base(connectionString, "images", logger)
        {
        }

        async Task<ImageData> IImageStorageService.GetImage(string imageName)
        {
            try
            {
                var blob = _container.GetBlobClient(imageName);
                var doesBlobExist = await blob.ExistsAsync();
                if (!doesBlobExist.Value) return null;

                var streamingResponse = await blob.DownloadStreamingAsync();
                if (streamingResponse?.Value == null) return null;

                return new ImageData
                {
                    Name = imageName,
                    Stream = streamingResponse.Value.Content,
                    ContentType = streamingResponse.Value.Details?.ContentType,
                    ContentDisposition = streamingResponse.Value.Details?.ContentDisposition
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(message: ex.Message);
                return null;
            }
        }
    }
}
