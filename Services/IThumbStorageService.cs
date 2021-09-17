using ImageResize.Models;
using System.Threading.Tasks;

namespace ImageResize.Services
{
    public interface IThumbStorageService
    {
        Task<ThumbUploadData> Upload(ImageData thumbData);
    }
}
