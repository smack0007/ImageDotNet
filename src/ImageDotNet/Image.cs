using System;
using System.IO;

namespace ImageDotNet
{
    public class Image
    {
        private readonly byte[] pixels;

        public int Width { get; private set; }

        public int Height { get; private set; }

        public PixelFormat Format { get; private set; }

        public int BytesPerPixel => (int)this.Format;

        public int Length => this.pixels.Length;

        public byte this[int index]
        {
            get { return this.pixels[index]; }
        }
        
        public Image(int width, int height, PixelFormat pixelFormat, byte[] pixels)
        {
            this.Width = width;
            this.Height = height;
            this.Format = pixelFormat;
            this.pixels = pixels;
        }

        public static ImageFormat GetImageFormatFromFileName(string fileName)
        {
            string fileExtension = Path.GetExtension(fileName).Substring(1).ToLower();

            switch (fileExtension)
            {
                case "tga":
                    return ImageFormat.Tga;
            }

            throw new ImageFormatException($"ImageFormat cannot be determined for file '{fileName}'. Use overload which specifies the ImageFormat.");
        }

        public static Image FromFile(string fileName)
        {
            return FromFile(fileName, GetImageFormatFromFileName(fileName));
        }

        public static Image FromFile(string fileName, ImageFormat format)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                return FromStream(stream, format);
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
