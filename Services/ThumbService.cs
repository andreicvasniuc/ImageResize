using ImageResize.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace ImageResize.Services
{
    public class ThumbService : IThumbService
    {
        private readonly ILogger<ThumbService> _logger;
        private readonly ILogStorageService _logStorageService;
        private readonly IImageStorageService _imageStorageService;
        private readonly IThumbStorageService _thumbStorageService;

        public ThumbService(IImageStorageService imageStorageService, IThumbStorageService thumbStorageService, ILogStorageService logStorageService, ILogger<ThumbService> logger)
        {
            _logger = logger;
            _logStorageService = logStorageService;
            _imageStorageService = imageStorageService;
            _thumbStorageService = thumbStorageService;
        }

        async Task<string> IThumbService.CreateAndUpload(string imageName)
        {
            if (string.IsNullOrEmpty(imageName)) return null;

            var imageData = await _imageStorageService.GetImage(imageName);
            if (imageData == null) return null;

            var thumbStream = CreateThumbStream(imageStream: imageData.Stream);
            if (thumbStream == null) return null;

            var thumbData = new ImageData
            {
                Name = imageName,
                Stream = thumbStream,
                ContentType = imageData.ContentType,
                ContentDisposition = imageData.ContentDisposition
            };
            var thumbUploadData = await _thumbStorageService.Upload(thumbData);
            if (thumbUploadData == null) return null;

            if(thumbUploadData.HasUploadedSuccessfuly)
            {
                LogThumbCreation(imageName);
            }

            return thumbUploadData.ThumbUrl;
        }

        private void LogThumbCreation(string imageName)
        {
            _logStorageService.Log(imageName);
        }

        private Stream CreateThumbStream(Stream imageStream)
        {
            try
            {
                var image = Image.FromStream(imageStream);
                var thumb = image.GetThumbnailImage(thumbHeight: 64, thumbWidth: 64, callback: null, callbackData: IntPtr.Zero);

                var thumbStream = new MemoryStream();
                thumb.Save(thumbStream, image.RawFormat);
                thumbStream.Position = 0;

                return thumbStream;
            }
            catch (Exception ex)
            {
                _logger.LogError(message: ex.Message);
                return null;
            }

        }
    }
}
