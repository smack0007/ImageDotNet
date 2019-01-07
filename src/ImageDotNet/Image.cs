using System.IO;
using System.Linq;

namespace ImageDotNet
{
    /// <summary>
    /// Acts a static gateway into the api.
    /// </summary>
    public static partial class Image
    {
        public static IImage Load(string fileName)
        {
            Guard.NotNull(fileName, nameof(fileName));

            using (var file = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                return Load(file);
            }
        }

        public static IImage Load(Stream stream)
        {
            Guard.NotNull(stream, nameof(stream));            

            if (IsPng(stream))
            {
                return LoadPng(stream);
            }
            else if (IsTga(stream))
            {
                return LoadTga(stream);
            }
            
            throw new ImageDotNetException("Unable to determine image type.");
        }

        public static IImage LoadByFileExtension(string fileName)
        {
            Guard.NotNull(fileName, nameof(fileName));

            using (var file = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                return LoadByFileExtension(fileName, file);
            }
        }

        public static IImage LoadByFileExtension(string fileName, Stream stream)
        {
            Guard.NotNull(fileName, nameof(fileName));
            Guard.NotNull(stream, nameof(stream));

            var fileExtension = Path.GetExtension(fileName);

            if (PngExtensions.Contains(fileExtension))
            {
                return LoadPng(stream);
            }
            else if (TgaExtensions.Contains(fileExtension))
            {
                return LoadTga(stream);
            }

            throw new ImageDotNetException("Unable to determine image type.");
        }
    }
}
