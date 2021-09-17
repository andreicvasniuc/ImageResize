using System.IO;

namespace ImageResize.Models
{
    public class ImageData
    {
        public Stream Stream { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public string ContentDisposition { get; set; }
    }
}
