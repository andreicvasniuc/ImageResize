using ImageResize.Models;
using System.Threading.Tasks;

namespace ImageResize.Services
{
    public interface IImageStorageService
    {
        Task<ImageData> GetImage(string imageName);
    }
}
