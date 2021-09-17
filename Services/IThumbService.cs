using System.Threading.Tasks;

namespace ImageResize.Services
{
    public interface IThumbService
    {
        Task<string> CreateAndUpload(string imageName);
    }
}
