using System.Threading.Tasks;

namespace ImageResize.Services
{
    public interface ILogStorageService
    {
        Task<bool> Log(string imageName);
    }
}
