using ImageResize.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ImageResize.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ThumbController : ControllerBase
    {
        private readonly IThumbService _thumbService;

        public ThumbController(IThumbService thumbService) => _thumbService = thumbService;

        [HttpPost("create")]
        public async Task<string> Create(string imageName) => await _thumbService.CreateAndUpload(imageName);
    }
}
