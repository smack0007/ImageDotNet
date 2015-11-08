using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpImage
{
    public class Image
    {
        public int Width { get; private set; }

        public int Height { get; private set; }

        public PixelFormat PixelFormat { get; private set; }

        public byte[] Pixels { get; private set; }

        public Image(int width, int height, PixelFormat pixelFormat, byte[] pixels)
        {
            this.Width = width;
            this.Height = height;
            this.PixelFormat = pixelFormat;
            this.Pixels = pixels;
        }

        public static ImageFormat GetImageFormatFromFileName(string fileName)
        {
            string fileExtension = Path.GetExtension(fileName).Substring(1).ToLower();

            switch (fileExtension)
            {
                case "tga":
                    return ImageFormat.Tga;
            }

            throw new InvalidOperationException($"ImageFormat cannot be determined for file {fileName}. Use overload which specifies the ImageFormat.");
        }

        public static Image FromFile(string fileName)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                return FromStream(stream, GetImageFormatFromFileName(fileName));
            }
        }

        public static Image FromStream(Stream stream, ImageFormat format)
        {
            switch (format)
            {
                case ImageFormat.Tga:
                    return Tga.FromStream(stream);
            }

            throw new NotImplementedException("ImageFormat." + format.ToString() + " not implemented.");
        }
    }
}
